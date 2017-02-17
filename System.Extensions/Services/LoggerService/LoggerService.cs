// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Diagnostics;
	using System.IO;
	using System.Text;

	/// <summary>
	/// LoggerService implementation.
	/// </summary>
	public class LoggerService : ILoggerService
	{
		#region [ Members ]

		private readonly static LoggerServiceSection _config = null;

		private readonly object _locker = new object();

		private string _logSource = null;

		private readonly static string _currentNamespace = typeof(LoggerService).FullName;

		#endregion

		#region [ Constants ]

		private static readonly string LINE_FORMAT = "[{0}] [{1}] [{2}] {3}\r\n";
		private static readonly string DATE_FORMAT = "yyyy-MM-dd, HH:mm:ss.ffff";

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Static constructor.
		/// </summary>
		static LoggerService()
		{
			_config = LoggerServiceSectionHandler.GetSection();

		}

		/// <summary>
		/// Default constructor. Uses &lt;loggerServiceConfiguration&gt; section from application configuration file.
		/// </summary>
		public LoggerService()
		{
			if (_config == null)
			{
				ThrowException.ThrowConfigurationErrorsException("Cannot find the loggerServiceConfiguration section from the configuration file");
			}

			this.LogSource = _config.LogSource;
			this.IsEnabled = _config.IsEnabled;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="logSource">The path of the LOG file.</param>
		public LoggerService(string logSource)
		{
			this.LogSource = logSource;
			this.IsEnabled = true;
		}

		#endregion

		#region [ ILoggerService Implementation ]

		/// <summary>
		/// Gets or sets the path of the LOG file.
		/// </summary>
		public string LogSource
		{
			get { return _logSource; }
			set
			{
				_logSource = value;

				if (!Path.IsPathRooted(_logSource))
				{
					_logSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logSource);
				}

				this.IsEnabled &= !string.IsNullOrEmpty(this.LogSource);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating whether the logger is enabled.
		/// </summary>
		public bool IsEnabled { get; set; }

		/// <summary>
		/// Write a message in the LOG.
		/// </summary>
		/// 
		/// <param name="instance">
		/// Instance to the caller (enter the keyword 'this').
		/// </param>
		/// 
		/// <param name="logStatus">
		/// Log status (use LogStatus static class).
		/// </param>
		/// 
		/// <param name="message">
		/// Log entry status.
		/// </param>
		/// 
		/// <param name="args">
		/// Optional parameters to format the message to write.
		/// </param>
		public void WriteLine(object instance, LogStatusEnum logStatus, string message, params object[] args)
		{
			if (this.IsEnabled)
			{
				this.WriteLog(instance, message, logStatus, args);
			}
		}

		/// <summary>
		/// Trace an exception.
		/// </summary>
		/// 
		/// <param name="instance">
		/// Instance to the caller (enter the keyword 'this').
		/// </param>
		/// 
		/// <param name="exception">
		/// Execption object.
		/// </param>
		/// 
		/// <param name="description">
		/// Optional description.
		/// </param>
		public void TraceException(object instance, Exception exception, string description = null)
		{
			if (this.IsEnabled)
			{
				StringBuilder sbMessage = new StringBuilder();

				if (description != null)
				{
					sbMessage.AppendFormat("{0} -> ", description);
				}

				sbMessage.Append(exception.ToString());

				if (Debugger.IsAttached) Debugger.Break(); // for debug purposes (you can comment this line)

				this.WriteLog(instance, sbMessage.ToString(), LogStatusEnum.Error);
			}
		}

		private void WriteLog(object instance, string message, LogStatusEnum logStatus, params object[] args)
		{
			if (args != null && args.Length != 0)
			{
				message = string.Format(message, args);
			}

			message = string.Format(LINE_FORMAT, DateTime.Now.ToString(DATE_FORMAT), logStatus, GetCallerMemberName(instance), message);

			lock (_locker)
			{
				try
				{
					var dirPath = Path.GetDirectoryName(_logSource);
					if (!Directory.Exists(dirPath))
					{
						Directory.CreateDirectory(dirPath);
					}

					File.AppendAllText(_logSource, message, Encoding.Default);
				}
				catch
				{
					if (Debugger.IsAttached) Debugger.Break();
				}
			}

#if DEBUG
			Debug.WriteLine(message);
#endif
		}

		private static string GetCallerMemberName(object instance)
		{
			if (instance == null) return "?";
			if (instance is string) return (string)instance;

			string methodName = null;
			
			var frames = new StackTrace().GetFrames();

			if (frames[1].GetMethod().DeclaringType.FullName != _currentNamespace)
			{
				methodName = frames[1].GetMethod().Name;
			}
			else if (frames[2].GetMethod().DeclaringType.FullName != _currentNamespace)
			{
				methodName = frames[2].GetMethod().Name;
			}
			else
			{
				methodName = frames[3].GetMethod().Name;
			}

			string className = instance.GetType().Name.Replace("`1", "<T>");

			return string.Format("{0}::{1}()", className, methodName);
		}

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Quick access to the ILoggerService defined in the configuration file (ServiceLocator definition).
	/// </summary>
	public class LoggerServiceHelper : Singleton<LoggerServiceHelper>, ILoggerService
	{
		#region [ Members ]

		private static readonly ILoggerService _service = ServiceLocator.Current.Resolve<ILoggerService>();

		#endregion

		#region [ ILoggerService Implementation ]

		/// <summary>
		/// Gets or sets the source of the LOG (file path, eventlog's name, etc).
		/// </summary>
		public string LogSource
		{
			get { return _service.LogSource; }
			set { _service.LogSource = value; }
		}

		/// <summary>
		/// Gets or sets the value indicating whether the logger is enabled.
		/// </summary>
		public bool IsEnabled
		{
			get { return _service.IsEnabled; }
			set { _service.IsEnabled = value; }
		}

		/// <summary>
		/// Write a message in the LOG.
		/// </summary>
		/// 
		/// <param name="instance">
		/// Instance to the caller (enter the keyword 'this').
		/// </param>
		/// 
		/// <param name="logStatus">
		/// Log entry status.
		/// </param>
		/// 
		/// <param name="message">
		/// Message to write.
		/// </param>
		/// 
		/// <param name="args">
		/// Optional parameters to format the message to write.
		/// </param>
		public void WriteLine(object instance, LogStatusEnum logStatus, string message, params object[] args)
		{
			_service.WriteLine(instance, logStatus, message, args);
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
			_service.TraceException(instance, exception, description);
		}

		#endregion
	}
}

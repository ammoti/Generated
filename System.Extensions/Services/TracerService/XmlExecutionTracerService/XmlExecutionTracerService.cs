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
	using System.Reflection;
	using System.Xml;

	/// <summary>
	/// Token enumeration.
	/// </summary>
	internal static class XmlToken
	{
		public static readonly string ExecutionTraceNode = "ExecutionTrace";
		public static readonly string TagNode = "Tag";

		public static readonly string ModuleAttribute = "module";
		public static readonly string ClassNameAttribute = "className";
		public static readonly string MethodNameAttribute = "methodName";
		public static readonly string MinDurationAttribute = "minDuration";
		public static readonly string MaxDurationAttribute = "maxDuration";
		public static readonly string AvgDurationAttribute = "avgDuration";
		public static readonly string TotalDurationAttribute = "totalDuration";
		public static readonly string CounterAttribute = "counter";
		public static readonly string LastCallAttribute = "lastCall";

	}

	public class XmlExecutionTracerService : ITracerService
	{
		#region [ Members ]

		private static readonly XmlExecutionTracerServiceSection _config = null;
		private static readonly object _locker = null;

		private static string _outputPath = null;

		private readonly Stopwatch _watcher = null;
		private readonly string _tag = null;

		private static readonly string DATE_FORMAT = "yyyy-MM-dd, HH:mm:ss";

		#endregion

		#region [ Constructors ]

		static XmlExecutionTracerService()
		{
			_locker = new object();

			_config = XmlExecutionTracerServiceSectionHandler.GetSection();
			if (_config != null)
			{
				_outputPath = _config.OutputPath;
				if (!Path.IsPathRooted(_outputPath))
				{
					_outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _outputPath);
				}
			}
		}

		public XmlExecutionTracerService(string tag = null)
		{
			_tag = tag;

			_watcher = new Stopwatch();
			_watcher.Start();
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~XmlExecutionTracerService()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the output path of the trace.
		/// </summary>
		public static string OutputPath
		{
			get { return _outputPath; }
			set { _outputPath = Path.GetFullPath(value); }
		}

		#endregion

		#region [ IDisposable Implementation ]

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">For internal use.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}

			if (_config.IsEnabled)
			{
				this.DoTrace();
			}
		}

		#endregion

		#region [ Private Methods ]

		/// <summary>
		/// Performs tracing operations.
		/// </summary>
		private void DoTrace()
		{
			_watcher.Stop();

			try
			{
				lock (_locker)
				{
					var dirPath = Path.GetDirectoryName(_outputPath);
					if (!Directory.Exists(dirPath))
					{
						Directory.CreateDirectory(dirPath);
					}

					XmlDocument xmlTrace = new XmlDocument();

					if (!File.Exists(_outputPath))
					{
						xmlTrace.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><ExecutionTraces></ExecutionTraces>");
					}
					else
					{
						xmlTrace.Load(_outputPath);
					}

					MethodBase mb = new StackFrame(3).GetMethod();

					string module = mb.Module.Name;
					string className = mb.ReflectedType.ToString();
					string method = mb.Name;
					long duration = _watcher.ElapsedMilliseconds;

					XmlNode rootNode = xmlTrace.DocumentElement;

					XmlNode node = xmlTrace.SelectSingleNode(string.Format(
						"//ExecutionTraces/ExecutionTrace[ @module = '{0}' and @className = '{1}' and @methodName = '{2}' and Tag = '{3}' ]",
						module, className, method, _tag));

					long counter = 1;
					long totalDuration = duration;
					string lastCall = DateTime.Now.ToString(DATE_FORMAT);

					if (_config.WithDebugTrace)
					{
						Debug.WriteLine(string.Format("({0}) [{1}ms] {2} {3}.{4}({5})",
							lastCall, _watcher.ElapsedMilliseconds.ToString().PadLeft(5, ' '), module, className, method, _tag));
					}

					if (node != null)
					{
						counter = Convert.ToInt64(node.Attributes[XmlToken.CounterAttribute].Value) + 1;
						long avgDuration = (Convert.ToInt64(node.Attributes[XmlToken.AvgDurationAttribute].Value) * counter + duration) / (counter + 1);
						totalDuration = Convert.ToInt64(node.Attributes[XmlToken.TotalDurationAttribute].Value) + duration;
						lastCall = node.Attributes[XmlToken.LastCallAttribute].Value;

						node.Attributes[XmlToken.CounterAttribute].Value = counter.ToString();
						node.Attributes[XmlToken.AvgDurationAttribute].Value = avgDuration.ToString();
						node.Attributes[XmlToken.TotalDurationAttribute].Value = totalDuration.ToString();

						long curMinDuration = Convert.ToInt64(node.Attributes[XmlToken.MinDurationAttribute].Value);
						long curMaxDuration = Convert.ToInt64(node.Attributes[XmlToken.MaxDurationAttribute].Value);

						if (curMinDuration == 0 || duration < curMinDuration)
						{
							node.Attributes[XmlToken.MinDurationAttribute].Value = duration.ToString();
						}

						if (duration > curMaxDuration)
						{
							node.Attributes[XmlToken.MaxDurationAttribute].Value = duration.ToString();
						}

						node.Attributes[XmlToken.LastCallAttribute].Value = lastCall;
					}
					else
					{
						XmlAttribute xmlAttrModule = xmlTrace.CreateAttribute(XmlToken.ModuleAttribute);
						xmlAttrModule.Value = module;

						XmlAttribute xmlAttrClassName = xmlTrace.CreateAttribute(XmlToken.ClassNameAttribute);
						xmlAttrClassName.Value = className;

						XmlAttribute xmlAttrMethodName = xmlTrace.CreateAttribute(XmlToken.MethodNameAttribute);
						xmlAttrMethodName.Value = method;

						XmlAttribute xmlAttrMinDuration = xmlTrace.CreateAttribute(XmlToken.MinDurationAttribute);
						xmlAttrMinDuration.Value = duration.ToString();

						XmlAttribute xmlAttrMaxDuration = xmlTrace.CreateAttribute(XmlToken.MaxDurationAttribute);
						xmlAttrMaxDuration.Value = duration.ToString();

						XmlAttribute xmlAttrDuration = xmlTrace.CreateAttribute(XmlToken.AvgDurationAttribute);
						xmlAttrDuration.Value = duration.ToString();

						XmlAttribute xmlAttrTotalDuration = xmlTrace.CreateAttribute(XmlToken.TotalDurationAttribute);
						xmlAttrTotalDuration.Value = totalDuration.ToString();

						XmlAttribute xmlAttrCounter = xmlTrace.CreateAttribute(XmlToken.CounterAttribute);
						xmlAttrCounter.Value = counter.ToString();

						XmlAttribute xmlAttrLastCall = xmlTrace.CreateAttribute(XmlToken.LastCallAttribute);
						xmlAttrLastCall.Value = lastCall;

						XmlElement xmlTag = xmlTrace.CreateElement(XmlToken.TagNode);

						XmlCDataSection xmlCdataTag = xmlTrace.CreateCDataSection(XmlToken.TagNode);
						xmlCdataTag.Value = _tag;

						xmlTag.AppendChild(xmlCdataTag);

						XmlElement xmlExecutionTrace = xmlTrace.CreateElement(XmlToken.ExecutionTraceNode);

						xmlExecutionTrace.Attributes.Append(xmlAttrModule);
						xmlExecutionTrace.Attributes.Append(xmlAttrClassName);
						xmlExecutionTrace.Attributes.Append(xmlAttrMethodName);
						xmlExecutionTrace.Attributes.Append(xmlAttrMinDuration);
						xmlExecutionTrace.Attributes.Append(xmlAttrMaxDuration);
						xmlExecutionTrace.Attributes.Append(xmlAttrDuration);
						xmlExecutionTrace.Attributes.Append(xmlAttrTotalDuration);
						xmlExecutionTrace.Attributes.Append(xmlAttrCounter);
						xmlExecutionTrace.Attributes.Append(xmlAttrLastCall);

						xmlExecutionTrace.AppendChild(xmlTag);

						rootNode.AppendChild(xmlExecutionTrace);
					}

					xmlTrace.Save(_outputPath);
				}
			}
			finally
			{
				/* Nothing */
			}
		}

		#endregion
	}
}

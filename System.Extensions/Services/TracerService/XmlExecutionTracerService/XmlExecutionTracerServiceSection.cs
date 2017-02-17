// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Xml;

	/// <summary>
	/// XmlExecutionTracerServiceSection class.
	/// </summary>
	public class XmlExecutionTracerServiceSection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public XmlExecutionTracerServiceSection()
		{
		}

		/// <summary>
		/// Loads the queueExecutionTracerServiceConfiguration Xml section.
		/// </summary>
		/// 
		/// <param name="sectionNode">
		/// The queueExecutionTracerServiceConfiguration root node.
		/// </param>
		internal void Load(XmlNode sectionNode)
		{
			if (sectionNode.Attributes["isEnabled"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute on queueExecutionTracerServiceConfiguration section");
			}

			if (sectionNode.Attributes["withDebugTrace"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'withDebugTrace' attribute on queueExecutionTracerServiceConfiguration section");
			}

			if (sectionNode.Attributes["outputPath"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'outputPath' attribute on queueExecutionTracerServiceConfiguration section");
			}

			string sIsEnabled = sectionNode.Attributes["isEnabled"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute value on queueExecutionTracerServiceConfiguration section");
			}

			string sWithDebugTrace = sectionNode.Attributes["withDebugTrace"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'withDebugTrace' attribute value on queueExecutionTracerServiceConfiguration section");
			}

			string sOutputPath = sectionNode.Attributes["outputPath"].Value;
			if (string.IsNullOrEmpty(sOutputPath))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'outputPath' attribute value on queueExecutionTracerServiceConfiguration section");
			}

			this.IsEnabled = Convert.ToBoolean(sIsEnabled);

			this.OutputPath = sOutputPath;

#if DEBUG
			this.WithDebugTrace = Convert.ToBoolean(sWithDebugTrace);
#else
			this.WithDebugTrace = false;
#endif
		}

		#region [ Properties ]

		/// <summary>
		/// Gets the value indicating whether the tracer is enabled.
		/// </summary>
		public bool IsEnabled
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the value indicating whether the DEBUG tracer is disabled.
		/// In RELEASE mode the WithDebugTrace value is always false.
		/// </summary>
		public bool WithDebugTrace
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the output file path of the Xml tracer.
		/// </summary>
		public string OutputPath
		{
			get;
			private set;
		}

		#endregion
	}
}

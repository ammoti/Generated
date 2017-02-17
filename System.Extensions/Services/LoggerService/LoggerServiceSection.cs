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
	/// LoggerServiceSection class.
	/// </summary>
	public class LoggerServiceSection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoggerServiceSection()
		{
		}

		/// <summary>
		/// Loads the loggerServiceConfiguration Xml section.
		/// </summary>
		/// <param name="sectionNode">The loggerServiceConfiguration root node.</param>
		internal void Load(XmlNode sectionNode)
		{
			if (sectionNode.Attributes["isEnabled"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute on loggerServiceConfiguration section");
			}

			if (sectionNode.Attributes["logSource"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'logSource' attribute on loggerServiceConfiguration section");
			}

			string sIsEnabled = sectionNode.Attributes["isEnabled"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute value on loggerServiceConfiguration section");
			}

			string sLogSource = sectionNode.Attributes["logSource"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'logSource' attribute value on loggerServiceConfiguration section");
			}

			this.IsEnabled = Convert.ToBoolean(sIsEnabled);
			this.LogSource = sLogSource;
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
		/// Gets or sets the path of the LOG file.
		/// </summary>
		public string LogSource
		{
			get;
			private set;
		}

		#endregion
	}
}

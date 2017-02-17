// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Configuration;
	using System.Xml;

	/// <summary>
	/// LoggerServiceSectionHandler class.
	/// </summary>
	public class LoggerServiceSectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the loggerServiceConfiguration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The LoggerServiceSection handler object.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			LoggerServiceSection cfgSection = new LoggerServiceSection();

			cfgSection.Load(section);
			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the LoggerServiceSection object.
		/// </summary>
		/// <returns>The LoggerServiceSection object.</returns>
		public static LoggerServiceSection GetSection()
		{
			return ConfigurationManager.GetSection("loggerServiceConfiguration") as LoggerServiceSection;
		}
	}
}

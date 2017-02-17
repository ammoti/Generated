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
	/// ServiceLocatorSectionHandler class.
	/// </summary>
	public class ServiceLocatorSectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the serviceLocatorConfiguration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The ServiceLocatorSection handler object.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			ServiceLocatorSection cfgSection = new ServiceLocatorSection();

			cfgSection.Load(section);
			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the ServiceLocatorSection object.
		/// </summary>
		/// <returns>The ServiceLocatorSection object.</returns>
		public static ServiceLocatorSection GetSection()
		{
			return ConfigurationManager.GetSection("serviceLocatorConfiguration") as ServiceLocatorSection;
		}
	}
}

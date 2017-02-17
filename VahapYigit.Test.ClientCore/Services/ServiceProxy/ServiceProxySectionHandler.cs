// -----------------------------------------------
// This file is part of the VahapYigit.Test.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.ClientCore
{
	using System.Configuration;
	using System.Xml;

	/// <summary>
	/// ServiceProxySectionHandler class.
	/// </summary>
	public class ServiceProxySectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the serviceProxyConfiguration section handler.
		/// </summary>
		/// 
		/// <param name="parent">
		/// Parent object.
		/// </param>
		/// 
		/// <param name="configContext">
		/// Configuration context object.
		/// </param>
		/// 
		/// <param name="section">
		/// Section XML node.
		/// </param>
		/// 
		/// <returns>
		/// The ServiceProxySection handler object.
		/// </returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			ServiceProxySection cfgSection = new ServiceProxySection();

			cfgSection.Load(section);
			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the ServiceProxySection object.
		/// </summary>
		/// 
		/// <returns>
		/// The ServiceProxySection object.
		/// </returns>
		public static ServiceProxySection GetSection()
		{
			return ConfigurationManager.GetSection("serviceProxyConfiguration") as ServiceProxySection;
		}
	}
}

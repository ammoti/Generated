// -----------------------------------------------
// This file is part of the VahapYigit.Test.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Configuration;
	using System.Xml;

	/// <summary>
	/// WcfServiceTrackingSectionHandler class.
	/// </summary>
	public class WcfServiceTrackingSectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the wcfServiceTrackingConfiguration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The WcfServiceTrackingSection handler object.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			WcfServiceTrackingSection cfgSection = new WcfServiceTrackingSection();

			cfgSection.Load(section);
			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the WcfServiceTrackingSection object.
		/// </summary>
		/// <returns>The WcfServiceTrackingSection object.</returns>
		public static WcfServiceTrackingSection GetSection()
		{
			return ConfigurationManager.GetSection("wcfServiceTrackingConfiguration") as WcfServiceTrackingSection;
		}
	}
}

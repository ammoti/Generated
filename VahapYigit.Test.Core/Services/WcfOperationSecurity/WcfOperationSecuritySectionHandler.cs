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
	/// WcfOperationSecuritySectionHandler class.
	/// </summary>
	public class WcfOperationSecuritySectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the wcfOperationSecurityConfiguration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The WcfOperationSecuritySection handler object.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			WcfOperationSecuritySection cfgSection = new WcfOperationSecuritySection();

			cfgSection.Load(section);
			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the WcfOperationSecuritySection object.
		/// </summary>
		/// <returns>The WcfOperationSecuritySection object.</returns>
		public static WcfOperationSecuritySection GetSection()
		{
			return ConfigurationManager.GetSection("wcfOperationSecurityConfiguration") as WcfOperationSecuritySection;
		}
	}
}

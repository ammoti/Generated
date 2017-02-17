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
	/// SmtpServiceSectionHandler class.
	/// </summary>
	public class SmtpServiceSectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the smtpServiceConfiguration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The SmtpServiceSection handler object.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			SmtpServiceSection cfgSection = new SmtpServiceSection();

			cfgSection.Load(section);
			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the SmtpServiceSection object.
		/// </summary>
		/// <returns>The SmtpServiceSection object.</returns>
		public static SmtpServiceSection GetSection()
		{
			return ConfigurationManager.GetSection("smtpServiceConfiguration") as SmtpServiceSection;
		}
	}
}

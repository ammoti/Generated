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
	/// MailTemplateServiceSectionHandler class.
	/// </summary>
	public class MailTemplateServiceSectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the mailTemplateServiceConfiguration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The MailTemplateServiceSection handler object.</returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			MailTemplateServiceSection cfgSection = new MailTemplateServiceSection();

			cfgSection.Load(section);
			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the MailTemplateServiceSection object.
		/// </summary>
		/// <returns>The MailTemplateServiceSection object.</returns>
		public static MailTemplateServiceSection GetSection()
		{
			return ConfigurationManager.GetSection("mailTemplateServiceConfiguration") as MailTemplateServiceSection;
		}
	}
}

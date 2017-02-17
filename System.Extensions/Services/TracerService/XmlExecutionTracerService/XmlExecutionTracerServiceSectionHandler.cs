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
	/// XmlExecutionTracerServiceSectionHandler class.
	/// </summary>
	public class XmlExecutionTracerServiceSectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the xmlExecutionTracerServiceConfiguration section handler.
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
		/// The XmlExecutionTracerServiceSection handler object.
		/// </returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			XmlExecutionTracerServiceSection cfgSection = new XmlExecutionTracerServiceSection();
			cfgSection.Load(section);

			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the XmlExecutionTracerServiceSection object.
		/// </summary>
		/// 
		/// <returns>
		/// The XmlExecutionTracerServiceSection object.
		/// </returns>
		public static XmlExecutionTracerServiceSection GetSection()
		{
			return ConfigurationManager.GetSection("xmlExecutionTracerServiceConfiguration") as XmlExecutionTracerServiceSection;
		}
	}
}

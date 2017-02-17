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
	/// QueueExecutionTracerServiceSectionHandler class.
	/// </summary>
	public class QueueExecutionTracerServiceSectionHandler : IConfigurationSectionHandler
	{
		#region [ IConfigurationSectionHandler Implementation ]

		/// <summary>
		/// Creates the txtExecutionTracerServiceConfiguration section handler.
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
		/// The QueueExecutionTracerServiceSection handler object.
		/// </returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			QueueExecutionTracerServiceSection cfgSection = new QueueExecutionTracerServiceSection();
			cfgSection.Load(section);

			return cfgSection;
		}

		#endregion

		/// <summary>
		/// Gets the QueueExecutionTracerServiceSection object.
		/// </summary>
		/// 
		/// <returns>
		/// The QueueExecutionTracerServiceSection object.
		/// </returns>
		public static QueueExecutionTracerServiceSection GetSection()
		{
			return ConfigurationManager.GetSection("queueExecutionTracerServiceConfiguration") as QueueExecutionTracerServiceSection;
		}
	}
}

// -----------------------------------------------
// This file is part of the VahapYigit.Test.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Configuration;
	using System.Xml;

	/// <summary>
	/// WcfServiceTrackingSection class.
	/// </summary>
	public class WcfServiceTrackingSection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public WcfServiceTrackingSection()
		{
		}

		/// <summary>
		/// Loads the wcfServiceTrackingConfiguration Xml section.
		/// </summary>
		/// <param name="sectionNode">The serviceProxyConfiguration root node.</param>
		internal void Load(XmlNode sectionNode)
		{
			if (sectionNode.Attributes["isEnabled"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute on wcfServiceTrackingConfiguration section");
			}

			if (sectionNode.Attributes["withMessageLogging"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'withMessageLogging' attribute on wcfServiceTrackingConfiguration section");
			}

			string sIsEnabled = sectionNode.Attributes["isEnabled"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute value on wcfServiceTrackingConfiguration section");
			}

			this.IsEnabled = Convert.ToBoolean(sIsEnabled);

			string sWithMessageLogging = sectionNode.Attributes["withMessageLogging"].Value;
			if (string.IsNullOrEmpty(sWithMessageLogging))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'withMessageLogging' attribute value on wcfServiceTrackingConfiguration section");
			}

			this.WithMessageLogging = Convert.ToBoolean(sWithMessageLogging);
		}

		/// <summary>
		/// Gets the default instance of WcfServiceTrackingSection (with isEnabled == false)
		/// </summary>
		public static WcfServiceTrackingSection Default
		{
			get
			{
				return new WcfServiceTrackingSection { IsEnabled = true, WithMessageLogging = false };
			}
		}

		#region [ Properties ]

		/// <summary>
		/// Gets the value indicating whether the WcfServiceTracking is enabled.
		/// </summary>
		public bool IsEnabled
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the value indicating whether the WCF Messages are loggued.
		/// </summary>
		public bool WithMessageLogging
		{
			get;
			private set;
		}

		#endregion
	}
}

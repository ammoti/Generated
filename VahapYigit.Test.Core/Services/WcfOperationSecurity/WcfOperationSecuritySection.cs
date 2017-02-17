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
	/// WcfOperationSecuritySection class.
	/// </summary>
	public class WcfOperationSecuritySection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public WcfOperationSecuritySection()
		{
		}

		/// <summary>
		/// Loads the wcfOperationSecurityConfiguration Xml section.
		/// </summary>
		/// <param name="sectionNode">The serviceProxyConfiguration root node.</param>
		internal void Load(XmlNode sectionNode)
		{
			if (sectionNode.Attributes["isEnabled"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute on wcfOperationSecurityConfiguration section");
			}

			string sIsEnabled = sectionNode.Attributes["isEnabled"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute value on wcfOperationSecurityConfiguration section");
			}

			this.IsEnabled = Convert.ToBoolean(sIsEnabled);
		}

		/// <summary>
		/// Gets the default instance of WcfOperationSecuritySection (with isEnabled == false)
		/// </summary>
		public static WcfOperationSecuritySection Default
		{
			get
			{
				return new WcfOperationSecuritySection { IsEnabled = false };
			}
		}

		#region [ Properties ]

		/// <summary>
		/// Gets the value indicating whether the WcfOperationSecurity is enabled.
		/// </summary>
		public bool IsEnabled
		{
			get;
			private set;
		}

		#endregion
	}
}

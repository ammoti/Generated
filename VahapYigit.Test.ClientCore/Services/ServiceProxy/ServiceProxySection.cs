// -----------------------------------------------
// This file is part of the VahapYigit.Test.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.ClientCore
{
	using System;
	using System.Configuration;
	using System.Xml;

	/// <summary>
	/// ServiceProxySection class.
	/// </summary>
	public class ServiceProxySection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ServiceProxySection()
		{
		}

		/// <summary>
		/// Loads the serviceProxyConfiguration Xml section.
		/// </summary>
		/// 
		/// <param name="sectionNode">
		/// The serviceProxyConfiguration root node.
		/// </param>
		internal void Load(XmlNode sectionNode)
		{
			if (sectionNode.Attributes["isLocal"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isLocal' attribute on serviceProxyConfiguration section");
			}

			string sIsLocal = sectionNode.Attributes["isLocal"].Value;
			if (string.IsNullOrEmpty(sIsLocal))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isLocal' attribute value on serviceProxyConfiguration section");
			}

			this.IsLocal = Convert.ToBoolean(sIsLocal);
		}

		/// <summary>
		/// Gets the default instance of ServiceProxySection (with isLocal == false)
		/// </summary>
		public static ServiceProxySection Default
		{
			get
			{
				return new ServiceProxySection { IsLocal = false };
			}
		}

		#region [ Properties ]

		/// <summary>
		/// Gets the value indicating whether the ServiceProxy is local.
		/// </summary>
		public bool IsLocal
		{
			get;
			private set;
		}

		#endregion
	}
}

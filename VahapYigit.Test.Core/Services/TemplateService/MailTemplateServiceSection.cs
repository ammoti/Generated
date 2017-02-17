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
	/// MailTemplateServiceSection class.
	/// </summary>
	public class MailTemplateServiceSection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MailTemplateServiceSection()
		{
		}

		/// <summary>
		/// Loads the mailTemplateServiceConfiguration Xml section.
		/// </summary>
		/// <param name="sectionNode">The mailTemplateServiceConfiguration root node.</param>
		internal void Load(XmlNode sectionNode)
		{
			if (sectionNode.Attributes["path"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'path' attribute on mailTemplateServiceConfiguration section");
			}

			string path = sectionNode.Attributes["path"].Value;
			if (string.IsNullOrEmpty(path))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'path' attribute value on mailTemplateServiceConfiguration section");
			}

			this.Path = (!System.IO.Path.IsPathRooted(path)) ?
				System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path) : path;
		}

		#region [ Properties ]

		/// <summary>
		/// Gets the Path value.
		/// </summary>
		public string Path
		{
			get;
			private set;
		}

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;
	using System.Xml.Schema;

	public class XmlHelper
	{
		/// <summary>
		/// Validates a Xml file using a specified Xsd file.
		/// </summary>
		///
		/// <param name="targetNamespace">
		/// The schema targetNamespace property, or null to use the targetNamespace specified in the schema.
		/// </param>
		///
		/// <param name="xmlPath">
		/// The Xml file path to validate.
		/// </param>
		///
		/// <param name="xsdPath">
		/// The Xsd file path used for validation.
		/// </param>
		///
		/// <param name="errors">
		/// Validation errors (warnings & errors).
		/// </param>
		///
		/// <returns>
		/// True if the Xml file is validate; otherwise, false.
		/// </returns>
		public static bool ValidateXml(string targetNamespace, string xmlPath, string xsdPath, out IList<ValidationEventArgs> errors)
		{
			XmlSchemaSet schemaSet = new XmlSchemaSet();
			schemaSet.Add(targetNamespace, xsdPath);

			XmlSchema compiledSchema = null;

			foreach (XmlSchema schema in schemaSet.Schemas())
			{
				compiledSchema = schema;
			}

			XmlReaderSettings settings = new XmlReaderSettings();
			IList<ValidationEventArgs> validationErrors = new List<ValidationEventArgs>();

			settings.Schemas.Add(compiledSchema);
			settings.ValidationEventHandler += new ValidationEventHandler((s, e) => validationErrors.Add(e));
			settings.ValidationType = ValidationType.Schema;

			using (var reader = XmlReader.Create(xmlPath, settings))
			{
				while (reader.Read()) ;
			}

			errors = validationErrors;

			return errors.Count != 0;
		}

		/// <summary>
		/// Determines whether the Xml file is correct.
		/// </summary>
		/// 
		/// <param name="filePath">
		/// Path of the Xml file to test.
		/// </param>
		/// 
		/// <returns>
		/// True if the Xml file is correct; otherwise, false.
		/// </returns>
		public static bool ValidateXml(string filePath)
		{
			XmlDocument xmlDocument;

			return XmlHelper.ValidateXml(filePath, out xmlDocument);
		}

		/// <summary>
		/// Determines whether the Xml file is correct.
		/// </summary>
		/// 
		/// <param name="filePath">
		/// Path of the Xml file to test.
		/// </param>
		/// 
		/// <param name="xmlDocument">
		/// The XmlDeocument instance if the Xml file is correct.
		/// </param>
		/// 
		/// <returns>
		/// True if the Xml file is correct; otherwise, false.
		/// </returns>
		public static bool ValidateXml(string filePath, out XmlDocument xmlDocument)
		{
			xmlDocument = null;

			if (File.Exists(filePath))
			{
				try
				{
					xmlDocument = new XmlDocument();
					xmlDocument.Load(filePath);
				}
				catch
				{
					// Nothing.
				}
			}

			return xmlDocument != null;
		}
	}
}

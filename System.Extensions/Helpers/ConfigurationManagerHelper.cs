// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;

	public static class ConfigurationManagerHelper
	{
		/// <summary>
		/// Loads a specific configuration file.
		/// </summary>
		/// 
		/// <param name="configFilePath">
		/// Path of the configuration file to load.
		/// </param>
		/// 
		/// <returns>
		/// The Configuration instance.
		/// </returns>
		public static System.Configuration.Configuration LoadConfigurationFile(string configFilePath)
		{
			ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };

			return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
		}

		public static T GetValue<T>(string keyName)
		{
			if (string.IsNullOrEmpty(keyName))
			{
				ThrowException.ThrowArgumentNullException("keyName");
			}

			string value = ConfigurationManager.AppSettings[keyName];
			if (value == null)
			{
				ThrowException.ThrowConfigurationErrorsException(string.Format("Cannot find the '{0}' configuration key from the AppSettings section", keyName));
			}

			return TypeHelper.To<T>(value);
		}

		public static T GetValue<T>(string keyName, T defaultValue = default(T))
		{
			if (string.IsNullOrEmpty(keyName))
			{
				ThrowException.ThrowArgumentNullException("keyName");
			}

			T value = defaultValue;

			string sValue = ConfigurationManager.AppSettings[keyName];
			if (sValue != null)
			{
				value = TypeHelper.To<T>(sValue);
			}

			return value;
		}

		public static IEnumerable<T> GetValues<T>(string keyName, params char[] separators)
		{
			if (string.IsNullOrEmpty(keyName))
			{
				ThrowException.ThrowArgumentNullException("keyName");
			}

			if (separators.IsNullOrEmpty())
			{
				ThrowException.ThrowArgumentNullException("separators");
			}

			string values = ConfigurationManagerHelper.GetValue<string>(keyName);

			return new List<T>(values.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(i => TypeHelper.To<T>(i)));
		}
	}
}

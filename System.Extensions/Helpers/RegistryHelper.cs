// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using Microsoft.Win32;

	public static class RegistryHelper
	{
		/// <summary>
		/// Retrieves the value associated with the specified name, in the specified registry key.
		/// If the name is not found in the specified key, returns a default value that you provide, or null if the specified key does not exist.
		/// </summary>
		///
		/// <typeparam name="T">
		/// Type of the returned value.
		/// </typeparam>
		///
		/// <param name="keyName">
		/// The full registry path of the key, beginning with a valid registry root, such as "HKEY_CURRENT_USER".
		/// </param>
		///
		/// <param name="valueName">
		/// The name of the name/value pair.
		/// </param>
		///
		/// <param name="defaultValue">
		/// The value to return if valueName does not exist.
		/// </param>
		///
		/// <returns>
		/// null if the subkey specified by keyName does not exist; otherwise, the value associated with valueName, or defaultValue if valueName is not found.
		/// </returns>
		public static T GetValue<T>(string keyName, string valueName, T defaultValue = default(T))
		{
			return TypeHelper.To<T>(Registry.GetValue(keyName, valueName, defaultValue));
		}

		/// <summary>
		/// Retrieves the value associated with the specified name, in the specified registry key.
		/// If the name is not found in the specified key, returns a default value that you provide, or null if the specified key does not exist.
		/// </summary>
		///
		/// <param name="keyName">
		/// The full registry path of the key, beginning with a valid registry root, such as "HKEY_CURRENT_USER".
		/// </param>
		///
		/// <param name="valueName">
		/// The name of the name/value pair.
		/// </param>
		///
		/// <param name="defaultValue">
		/// The value to return if valueName does not exist.
		/// </param>
		///
		/// <returns>
		/// null if the subkey specified by keyName does not exist; otherwise, the value associated with valueName, or defaultValue if valueName is not found.
		/// </returns>
		public static object GetValue(string keyName, string valueName, object defaultValue = null)
		{
			return Registry.GetValue(keyName, valueName, defaultValue);
		}

		/// <summary>
		/// Sets the specified name/value pair on the specified registry key.
		/// If the specified key does not exist, it is created.
		/// </summary>
		///
		/// <param name="keyName">
		/// The full registry path of the key, beginning with a valid registry root, such as "HKEY_CURRENT_USER".
		/// </param>
		///
		/// <param name="valueName">
		/// The name of the name/value pair.
		/// </param>
		///
		/// <param name="value">
		/// The value to be stored.
		/// </param>
		public static void SetValue(string keyName, string valueName, object value)
		{
			Registry.SetValue(keyName, valueName, value);
		}

		/// <summary>
		/// Determines whether the value exists.
		/// </summary>
		///
		/// <param name="keyName">
		/// The full registry path of the key, beginning with a valid registry root, such as "HKEY_CURRENT_USER".
		/// </param>
		///
		/// <param name="valueName">
		/// The name of the name/value pair.
		/// </param>
		public static bool HasValue(string keyName, string valueName)
		{
			return GetValue(keyName, valueName) != null;
		}
	}
}
// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	public static class DictionaryExtensions
	{
		/// <summary>
		/// Adds or updates the specified key and value to the dictionary.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the value.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Dictionary.
		/// </param>
		/// 
		/// <param name="key">
		/// The key of the element to add or update.
		/// </param>
		/// 
		/// <param name="value">
		/// The value of the element to add or update. The value can be null for reference types.
		/// </param>
		public static void AddOrUpdate<T>(this Dictionary<string, T> source, string key, T value)
		{
			if (source.ContainsKey(key))
			{
				source[key] = value;
			}
			else
			{
				source.Add(key, value);
			}
		}
	}
}

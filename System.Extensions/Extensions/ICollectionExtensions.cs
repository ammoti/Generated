// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Linq;

	public static class ICollectionExtensions
	{
		/// <summary>
		/// Removes items from the collection that verify the predicate.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the collection.
		/// </typeparam>
		/// 
		/// <param name="collection">
		/// Collection.
		/// </param>
		/// 
		/// <param name="predicate">
		/// Predicate.
		/// </param>
		public static void RemoveWhen<T>(this ICollection<T> collection, Predicate<T> predicate)
		{
			if (collection != null)
			{
				var items = collection.Where(i => predicate(i)).ToList();
				items.ForEach(t => collection.Remove(t));
			}
		}
	}
}

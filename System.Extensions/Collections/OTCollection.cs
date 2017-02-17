// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;

	/// <summary>
	/// Observable T Collection.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// The collection object's type.
	/// </typeparam>
	[Serializable]
	[CollectionDataContract(Namespace = "System.Extensions.OTCollection")]
	public class OTCollection<T> : ObservableCollection<T>
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public OTCollection()
			: base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="source">Initial collection.</param>
		public OTCollection(IEnumerable<T> source)
			: base(source.ToList())
		{
		}

		/// <summary>
		/// Removes all the data of the current collection according to a predicate.
		/// </summary>
		/// 
		/// <param name="condition">
		/// The condition for which the data are removed.
		/// </param>
		public void RemoveWhere(Predicate<T> condition)
		{
			int i = 0;
			while (i < this.Count)
			{
				if (condition(this[i]))
				{
					this.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		/// <summary>
		/// Returns the string that represents the current instance.
		/// </summary>
		/// 
		/// <returns>
		/// The string that represents the current instance.
		/// </returns>
		public override string ToString()
		{
			return string.Join<T>(", ", base.Items);
		}

		/// <summary>
		/// Adds the elements of the specified collection to the end of the collection.
		/// </summary>
		/// 
		/// <param name="collection">
		/// The collection whose elements should be added to the end of the collection.
		/// The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.
		/// </param>
		public void AddRange(IEnumerable<T> items)
		{
			this.IsRangeAdding = true;

			foreach (T item in items)
			{
				if (item.Equals(items.Last()))
				{
					this.IsRangeAdding = false;
				}

				base.Add(item);
			}
		}

		public bool IsRangeAdding
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets the value indicating whteher the collection is empty or not.
		/// </summary>
		public bool IsEmpty
		{
			get { return this.Count > 0; }
		}
	}

	public static class OTCollectionExtensions
	{
		public static OTCollection<T> ToOTCollection<T>(this IEnumerable<T> source)
		{
			return new OTCollection<T>(source);
		}

		public static OTCollection<T> ToOTCollection<T>(this IEnumerable<IEnumerable<T>> sources)
		{
			if (sources == null)
				return null;

			List<T> list = new List<T>();
			foreach (IEnumerable<T> source in sources)
			{
				list.AddRange(source);
			}

			return list.ToOTCollection();
		}
	}
}

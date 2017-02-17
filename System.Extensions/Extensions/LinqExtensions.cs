// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public static class LinqExtensions
	{
		/// <summary>
		/// Determines whether a sequence is null or empty.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source sequence.
		/// </param>
		/// 
		/// <returns>
		/// True if the source sequence is null or empty; otherwise, false.
		/// </returns>
		public static bool IsNullOrEmpty(this IEnumerable source)
		{
			if (source == null)
				return true;

			if (source is IEnumerable)
			{
				var items = source as IEnumerable;
				if (!items.GetEnumerator().MoveNext())
					return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether a sequence is null or empty.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the elements of source.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Source sequence.
		/// </param>
		/// 
		/// <returns>
		/// True if the source sequence is null or empty; otherwise, false.
		/// </returns>
		public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
		{
			return source == null || source.Any() == false;
		}

		/// <summary>
		/// Executes foreach loop on the sequence and executes an action for each item of the sequence.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source sequence.
		/// </param>
		/// 
		/// <param name="action">
		/// Action to execute for each item of the source sequence.
		/// </param>
		public static void ForEach(this IEnumerable source, Action<object> action)
		{
			if (!source.IsNullOrEmpty())
			{
				foreach (var item in source)
					action(item);
			}
		}

		/// <summary>
		/// Executes foreach loop on the sequence and executes an action for each item of the sequence.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the elements of source.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Source sequence.
		/// </param>
		/// 
		/// <param name="action">
		/// Action to execute for each item of the source sequence.
		/// </param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (!source.IsNullOrEmpty())
			{
				foreach (T item in source)
					action(item);
			}
		}

		/// <summary>
		/// Executes asynchronous foreach loop on the sequence and executes an action for each item of the sequence.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the elements of source.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Source sequence.
		/// </param>
		/// 
		/// <param name="action">
		/// Action to execute for each item of the source sequence.
		/// </param>
		/// 
		/// <returns>
		/// A task that represents the completion of all of the supplied tasks.
		/// </returns>
		public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
		{
			if (!source.IsNullOrEmpty())
			{
				return Task.WhenAll(from item in source select Task.Run(() => action(item)));
			}

			return null;
		}

		public static IEnumerable<T> Contains<T>(this IEnumerable<T> source, IEnumerable<T> values, IEqualityComparer<T> comparer = null)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (values == null) throw new ArgumentNullException("values");

			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			return values.Where(i => source.Contains(i, comparer));
		}

		/// <summary>
		/// Usage: [ Single: collection.DistinctBy(p => p.Id) ] [ Composed: collection.DistinctBy(p => new { p.Id, p.Name }) ]
		/// </summary>
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			var seenKeys = new HashSet<TKey>();

			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		/// <summary>
		/// Determines whether a source contains duplicated elements given a duplication function.
		/// </summary>
		/// 
		/// <typeparam name="TSource">
		/// The type of the elements of source.
		/// </typeparam>
		/// 
		/// <typeparam name="TKey">
		/// Type of the property used to find duplicated elements.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Source sequence.
		/// </param>
		/// 
		/// <param name="keySelector">
		/// Duplication function (ex, f => f.Id).
		/// </param>
		/// 
		/// <returns>
		/// True if the source sequence contains duplicated elements; otherwise, false.
		/// </returns>
		public static bool ContainsDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			var families = source.ToLookup(keySelector);
			foreach (var family in families)
			{
				if (family.Count() > 1)
					return true;
			}

			return false;
		}
		/// <summary>
		/// Gets an HashSet from the sequence.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the elements of source.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Source sequence.
		/// </param>
		/// 
		/// <returns>
		/// The HashSet.
		/// </returns>
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
		{
			return (source == null) ? new HashSet<T>() : new HashSet<T>(source);
		}

		// https://code.google.com/p/morelinq/source/browse/MoreLinq/MaxBy.cs?r=29adeed018903e03e4ef0847c155908ef4444dd7&spec=svnfc530c948143586058a02062564b456e262ebe9f
		/// <summary>
		/// Returns the maximal element of the given sequence, based on
		/// the given projection.
		/// </summary>
		/// <remarks>
		/// If more than one element has the maximal projected value, the first
		/// one encountered will be returned. This overload uses the default comparer
		/// for the projected type. This operator uses immediate execution, but
		/// only buffers a single result (the current maximal element).
		/// </remarks>
		/// <typeparam name="TSource">Type of the source sequence</typeparam>
		/// <typeparam name="TKey">Type of the projected element</typeparam>
		/// <param name="source">Source sequence</param>
		/// <param name="selector">Selector to use to pick the results to compare</param>
		/// <returns>The maximal element, according to the projection.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
		/// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

		// https://code.google.com/p/morelinq/source/browse/MoreLinq/MaxBy.cs?r=29adeed018903e03e4ef0847c155908ef4444dd7&spec=svnfc530c948143586058a02062564b456e262ebe9f
		/// <summary>
		/// Returns the maximal element of the given sequence, based on
		/// the given projection and the specified comparer for projected values. 
		/// </summary>
		/// <remarks>
		/// If more than one element has the maximal projected value, the first
		/// one encountered will be returned. This overload uses the default comparer
		/// for the projected type. This operator uses immediate execution, but
		/// only buffers a single result (the current maximal element).
		/// </remarks>
		/// <typeparam name="TSource">Type of the source sequence</typeparam>
		/// <typeparam name="TKey">Type of the projected element</typeparam>
		/// <param name="source">Source sequence</param>
		/// <param name="selector">Selector to use to pick the results to compare</param>
		/// <param name="comparer">Comparer to use to compare projected values</param>
		/// <returns>The maximal element, according to the projection.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
		/// or <paramref name="comparer"/> is null</exception>
		/// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (selector == null) throw new ArgumentNullException("selector");
			if (comparer == null) throw new ArgumentNullException("comparer");

			using (var sourceIterator = source.GetEnumerator())
			{
				if (!sourceIterator.MoveNext())
				{
					throw new InvalidOperationException("Sequence was empty");
				}

				var max = sourceIterator.Current;
				var maxKey = selector(max);

				while (sourceIterator.MoveNext())
				{
					var candidate = sourceIterator.Current;
					var candidateProjected = selector(candidate);

					if (comparer.Compare(candidateProjected, maxKey) > 0)
					{
						max = candidate;
						maxKey = candidateProjected;
					}
				}

				return max;
			}
		}

		// https://code.google.com/p/morelinq/source/browse/MoreLinq/MinBy.cs?r=29adeed018903e03e4ef0847c155908ef4444dd7&spec=svnfc530c948143586058a02062564b456e262ebe9f
		/// <summary>
		/// Returns the minimal element of the given sequence, based on
		/// the given projection.
		/// </summary>
		/// <remarks>
		/// If more than one element has the minimal projected value, the first
		/// one encountered will be returned. This overload uses the default comparer
		/// for the projected type. This operator uses immediate execution, but
		/// only buffers a single result (the current minimal element).
		/// </remarks>
		/// <typeparam name="TSource">Type of the source sequence</typeparam>
		/// <typeparam name="TKey">Type of the projected element</typeparam>
		/// <param name="source">Source sequence</param>
		/// <param name="selector">Selector to use to pick the results to compare</param>
		/// <returns>The minimal element, according to the projection.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
		/// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		// https://code.google.com/p/morelinq/source/browse/MoreLinq/MinBy.cs?r=29adeed018903e03e4ef0847c155908ef4444dd7&spec=svnfc530c948143586058a02062564b456e262ebe9f
		/// <summary>
		/// Returns the minimal element of the given sequence, based on
		/// the given projection and the specified comparer for projected values.
		/// </summary>
		/// <remarks>
		/// If more than one element has the minimal projected value, the first
		/// one encountered will be returned. This overload uses the default comparer
		/// for the projected type. This operator uses immediate execution, but
		/// only buffers a single result (the current minimal element).
		/// </remarks>
		/// <typeparam name="TSource">Type of the source sequence</typeparam>
		/// <typeparam name="TKey">Type of the projected element</typeparam>
		/// <param name="source">Source sequence</param>
		/// <param name="selector">Selector to use to pick the results to compare</param>
		/// <param name="comparer">Comparer to use to compare projected values</param>
		/// <returns>The minimal element, according to the projection.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
		/// or <paramref name="comparer"/> is null</exception>
		/// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (selector == null) throw new ArgumentNullException("selector");
			if (comparer == null) throw new ArgumentNullException("comparer");

			using (var sourceIterator = source.GetEnumerator())
			{
				if (!sourceIterator.MoveNext())
				{
					throw new InvalidOperationException("Sequence was empty");
				}

				var min = sourceIterator.Current;
				var minKey = selector(min);

				while (sourceIterator.MoveNext())
				{
					var candidate = sourceIterator.Current;
					var candidateProjected = selector(candidate);

					if (comparer.Compare(candidateProjected, minKey) < 0)
					{
						min = candidate;
						minKey = candidateProjected;
					}
				}

				return min;
			}
		}
	}
}

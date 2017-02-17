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

	// Based on http://www.codeproject.com/Articles/62397/LINQ-to-Tree-A-Generic-Technique-for-Querying-Tree

	public static class ITreeNodeExtensions
	{
		#region [ Public Methods ]

		/// <summary>
		/// Returns a collection of child elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> Elements<TNode>(this ITreeNode<TNode> adapter)
		{
			foreach (var child in adapter.Children)
			{
				yield return child;
			}
		}

		/// <summary>
		/// Returns a collection of descendant elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> Descendants<TNode>(this ITreeNode<TNode> adapter)
		{
			foreach (var child in adapter.Children)
			{
				yield return child;

				foreach (var descendant in child.Descendants())
				{
					yield return descendant;
				}
			}
		}

		/// <summary>
		/// Returns a collection of ancestor elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> Ancestors<TNode>(this ITreeNode<TNode> adapter)
		{
			var parent = adapter.Parent;

			while (parent != null)
			{
				yield return parent;

				parent = parent.Parent;
			}
		}

		/// <summary>
		/// Returns a collection containing this element and child elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> SelfAndElements<TNode>(this ITreeNode<TNode> adapter)
		{
			yield return adapter;

			adapter.Elements();
		}

		/// <summary>
		/// Returns a collection containing this element and all descendant elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> SelfAndDescendants<TNode>(this ITreeNode<TNode> adapter)
		{
			yield return adapter;

			foreach (var descendant in adapter.Descendants())
			{
				yield return descendant;
			}
		}

		/// <summary>
		/// Returns a collection containing this element and all ancestor elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> SelfAndAncestors<TNode>(this ITreeNode<TNode> adapter)
		{
			yield return adapter;

			foreach (var child in adapter.Ancestors())
			{
				yield return child;
			}
		}

		/// <summary>
		/// Returns a collection of child elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> Elements<TNode>(this IEnumerable<ITreeNode<TNode>> items)
		{
			return items.DrillDown(i => i.Elements());
		}

		/// <summary>
		/// Returns a collection of descendant elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> Descendants<TNode>(this IEnumerable<ITreeNode<TNode>> items)
		{
			return items.DrillDown(i => i.Descendants());
		}

		/// <summary>
		/// Returns a collection of ancestor elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> Ancestors<TNode>(this IEnumerable<ITreeNode<TNode>> items)
		{
			return items.DrillDown(i => i.Ancestors());
		}

		/// <summary>
		/// Returns a collection containing these elements and child elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> SelfsAndElements<TNode>(this IEnumerable<ITreeNode<TNode>> items)
		{
			return items.DrillDown(i => i.Elements());
		}

		/// <summary>
		/// Returns a collection containing these elements and all the descendant elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> SelfsAndDescendants<TNode>(this IEnumerable<ITreeNode<TNode>> items)
		{
			return items.DrillDown(i => i.SelfAndDescendants());
		}

		/// <summary>
		/// Returns a collection containing these elements and all the ancestor elements.
		/// </summary>
		public static IEnumerable<ITreeNode<TNode>> SelfsAndAncestors<TNode>(this IEnumerable<ITreeNode<TNode>> items)
		{
			return items.DrillDown(i => i.SelfAndAncestors());
		}

		#endregion

		#region [ Private Methods ]

		/// <summary>
		/// Applies the given function to each of the items in the supplied IEnumerable.
		/// </summary>
		private static IEnumerable<ITreeNode<TNode>> DrillDown<TNode>(this IEnumerable<ITreeNode<TNode>> items, Func<ITreeNode<TNode>, IEnumerable<ITreeNode<TNode>>> function)
		{
			foreach (var item in items)
			{
				foreach (var childItem in function(item))
				{
					yield return childItem;
				}
			}
		}

		#endregion
	}
}

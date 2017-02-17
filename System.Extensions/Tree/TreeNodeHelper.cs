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

	public static class TreeNodeHelper
	{
		/// <summary>
		/// Transforms a flat collection to a hierarchy object.
		/// Elements must implements TreeNodeBase abstract class.
		/// </summary>
		/// 
		/// <typeparam name="TNode">
		/// Type of the items.
		/// </typeparam>
		/// 
		/// <param name="flatCollection">
		/// The collection that contains all the elements of a hierarchy.
		/// Elements must implements TreeNodeBase abstract class.
		/// </param>
		/// 
		/// <param name="idSelector">
		/// Func that returns the Id property of an item.
		/// </param>
		/// 
		/// <param name="idParentSelector">
		/// Func that returns the Parent Id property of an item.
		/// </param>
		/// 
		/// <param name="orderBySelector">
		/// Func that sorts items on same parent node.
		/// </param>
		/// 
		/// <returns>
		/// The hierarchy.
		/// </returns>
		public static IEnumerable<ITreeNode<TNode>> Hierarchize<TNode, TOrderKey>(
			IEnumerable<ITreeNode<TNode>> flatCollection,
			Func<TNode, long> idSelector,
			Func<TNode, long?> idParentSelector,
			Func<TNode, TOrderKey> orderBySelector = null) where TNode : class
		{
			if (flatCollection.IsNullOrEmpty())
			{
				return flatCollection;
			}

			var orderByFunc = new Func<IEnumerable<ITreeNode<TNode>>, IEnumerable<ITreeNode<TNode>>>(collection =>
			{
				return (orderBySelector != null) ?
					collection = collection.OrderBy(o => orderBySelector(o.Item)) :
					collection;
			});

			var dirtyFlatCollection = flatCollection.DeepCopy(); // not to modify flatCollection input

			foreach (var item in dirtyFlatCollection)
			{
				long? idParent = idParentSelector(item.Item);
				if (idParent != null)
				{
					var parent = dirtyFlatCollection.FirstOrDefault(i => idSelector(i.Item) == idParent);
					if (parent == null)
					{
						ThrowException.Throw("Cannot hierarchize the flatcollection because it is not complete (missing parent item with Id = {0}", idParent);
					}

					item.Parent = parent;
				}

				item.Children = orderByFunc(dirtyFlatCollection.Where(i => i != item && idParentSelector(i.Item) != null && idParentSelector(i.Item) == idSelector(item.Item)));
			}

			return orderByFunc(dirtyFlatCollection.Where(i => idParentSelector(i.Item) == null));
		}
	}
}

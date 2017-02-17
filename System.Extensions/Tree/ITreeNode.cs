// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	public interface ITreeNode<TNode>
	{
		/// <summary>
		/// Gets the Node.
		/// To set the value use a constructor with parameter.
		/// </summary>
		TNode Item { get; }

		/// <summary>
		/// Gets the parent of the Node.
		/// </summary>
		ITreeNode<TNode> Parent { get; set; }

		/// <summary>
		/// Gets the children of the Node.
		/// </summary>
		IEnumerable<ITreeNode<TNode>> Children { get; set; }
	}
}

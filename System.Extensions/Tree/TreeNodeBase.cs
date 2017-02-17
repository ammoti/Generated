// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	public abstract class TreeNodeBase<TNode> : ITreeNode<TNode> where TNode : class
	{
		#region [ Members ]

		protected readonly TNode _item = null;

		#endregion

		#region [ Constructors ]

		public TreeNodeBase(TNode item)
		{
			if (item == null)
			{
				ThrowException.ThrowArgumentNullException("item");
			}
			
			_item = item;
		}

		#endregion

		#region [ Public Methods ]

		public override string ToString()
		{
			return this.Item.ToString();
		}

		#endregion

		#region [ ITreeNode Implementation ]

		public TNode Item
		{
			get { return _item; }
		}

		public virtual ITreeNode<TNode> Parent { get; set; }

		public virtual IEnumerable<ITreeNode<TNode>> Children { get; set; }

		#endregion
	}
}

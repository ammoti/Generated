// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.IO
{
	using System.Collections.Generic;

	public class DirectoryTreeNodeAdapter : TreeNodeBase<DirectoryInfo>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="item">
		/// DirectoryInfo instance.
		/// </param>
		public DirectoryTreeNodeAdapter(DirectoryInfo item)
			: base(item)
		{
		}

		#region [ TreeNodeBase Implementation ]

		/// <summary>
		/// Gets the parent directory of the current directory.
		/// </summary>
		public override ITreeNode<DirectoryInfo> Parent
		{
			get { return new DirectoryTreeNodeAdapter(_item.Parent); }
			set { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets the children directories of the current directory.
		/// </summary>
		public override IEnumerable<ITreeNode<DirectoryInfo>> Children
		{
			get
			{
				foreach (var dir in _item.GetDirectories())
				{
					yield return new DirectoryTreeNodeAdapter(dir);
				}
			}
			set { throw new NotSupportedException(); }
		}

		#endregion
	}
}

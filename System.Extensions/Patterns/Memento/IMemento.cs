// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public interface IMemento<T> where T : class
	{
		/// <summary>
		///  Undo modifications.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source.
		/// </param>
		void Undo(ref T target);

		/// <summary>
		/// Redo undone modifications.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source.
		/// </param>
		void Redo(ref T target);
	}
}

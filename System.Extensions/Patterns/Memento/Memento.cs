// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Used to create a backup and rollback of all the public, readable, and writable properties of an object.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// Type of the source.
	/// </typeparam>
	public class Memento<T> : IMemento<T> where T : class
	{
		#region [ Members ]

		private T _undoBackup = null;
		private T _redoBackup = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="target">
		/// Target.
		/// </param>
		public Memento(T target)
		{
			_undoBackup = target.DeepCopy<T>();
		}

		#endregion

		#region [ IMemento<T> Implementation ]

		/// <summary>
		///  Undo modifications.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source.
		/// </param>
		public void Undo(ref T target)
		{
			if (_undoBackup != null)
			{
				_redoBackup = target;
				target = _undoBackup;
				_undoBackup = null;
			}
		}

		/// <summary>
		/// Redo undone modifications.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source.
		/// </param>
		public void Redo(ref T target)
		{
			if (_redoBackup != null)
			{
				_undoBackup = target;
				target = _redoBackup;
				_redoBackup = null;
			}
		}

		#endregion
	}
}

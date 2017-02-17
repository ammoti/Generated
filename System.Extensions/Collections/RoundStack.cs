// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Rounded stack with capacity.
	/// </summary>
	/// <typeparam name="T">Type of the items in the stack.</typeparam>
	[Serializable]
	public class RoundStack<T>
	{
		#region [ Members ]

		private int _top = 1;
		private int _bottom = 0;

		private T[] _items;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="capacity">
		/// The capacity of the RoundStack&lt;T&gt;.
		/// </param>
		public RoundStack(int capacity)
		{
			if (capacity < 1)
			{
				ThrowException.ThrowArgumentOutOfRangeException("capacity");
			}

			_items = new T[capacity + 1];
		}

		#endregion

		#region [ Public methods ]

		/// <summary>
		/// Removes and returns the item at the top of the RoundStack&lt;T&gt;.
		/// </summary>
		/// 
		/// <returns>
		/// The item at the top of the RoundStack&lt;T&gt;.
		/// </returns>
		public T Pop()
		{
			if (this.Count == 0)
			{
				ThrowException.ThrowInvalidOperationException("Cannot pop from an empty RoundStack");
			}

			T removed = _items[_top];
			_items[_top--] = default(T);

			if (_top < 0)
			{
				_top += _items.Length;
			}

			return removed;
		}

		/// <summary>
		/// Inserts an item at the top of the RoundStack&lt;T&gt;.
		/// </summary>
		/// 
		/// <param name="item">
		/// Item to insert at the top of the RoundStack&lt;T&gt;.
		/// </param>
		public void Push(T item)
		{
			if (this.IsFull)
			{
				_bottom++;

				if (_bottom >= _items.Length)
				{
					_bottom -= _items.Length;
				}
			}

			if (++_top >= _items.Length)
			{
				_top -= _items.Length;
			}

			_items[_top] = item;
		}

		/// <summary>
		/// Returns the item at the top of the RoundStack&lt;T&gt; without removing it.
		/// </summary>
		public T Peek()
		{
			return _items[_top];
		}

		/// <summary>
		/// Removes all the items from the RoundStack&lt;T&gt;.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < _items.Length; i++)
			{
				_items[i] = default(T);
			}

			_top = 1;
			_bottom = 0;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets if the RoundStack&lt;T&gt; is full.
		/// </summary>
		public bool IsFull
		{
			get
			{
				return _top == _bottom;
			}
		}

		/// <summary>
		/// Gets the number of items contained in the RoundStack&lt;T&gt;.
		/// </summary>
		public int Count
		{
			get
			{
				int count = _top - _bottom - 1;
				if (count < 0)
				{
					count += _items.Length;
				}

				return count;
			}
		}

		/// <summary>
		/// Gets the capacity of the RoundStack&lt;T&gt;.
		/// </summary>
		public int Capacity
		{
			get
			{
				return _items.Length - 1;
			}
		}

		#endregion
	}
}

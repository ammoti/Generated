// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Provides singleton pattern implementation.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// Type of the singleton.
	/// </typeparam>
	public class Singleton<T> where T : class, new()
	{
		#region [ Members ]

		/// <summary>
		/// Current singleton instance.
		/// </summary>
		private static readonly T _current = new T();

		/// <summary>
		/// Locker object for safe-thread operations.
		/// </summary>
		protected static readonly object _locker = null;

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Static constructor.
		/// </summary>
		static Singleton()
		{
			_locker = new object();
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Singleton()
		{
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the current instance.
		/// </summary>
		public static T Current
		{
			get
			{
				return _current;
			}
		}

		#endregion
	}
}

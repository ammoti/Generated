// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Concurrent;

	/// <summary>
	/// Provides multiton pattern implementation.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// Type of the multiton.
	/// </typeparam>
	public class Multiton<T> : Singleton<T> where T : class, new()
	{
		#region [ Members ]

		/// <summary>
		/// Inner collection to store instances.
		/// </summary>
		protected readonly ConcurrentDictionary<string, object> _container = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Multiton()
			: base()
		{
			_container = new ConcurrentDictionary<string, object>();
		}

		#endregion

		/// <summary>
		/// Adds an object instance in the multiton stored instances.
		/// </summary>
		/// 
		/// <param name="key">
		/// Instance key.
		/// </param>
		/// 
		/// <param name="instance">
		/// Object instance.
		/// </param>
		public void Add(string key, object instance)
		{
			lock (_locker)
			{
				if (_container.ContainsKey(key))
				{
					ThrowException.ThrowArgumentException("There is already an instance registered using key = '{0}'", key);
				}

				_container.TryAdd(key, instance);
			}
		}

		/// <summary>
		/// Gets an instance using key.
		/// </summary>
		/// 
		/// <param name="key">
		/// Instance key.
		/// </param>
		/// 
		/// <returns>
		/// The required instance.
		/// </returns>
		public T Get(string key)
		{
			object instance = null;

			_container.TryGetValue(key, out instance);

			return instance != null ? (T)instance : default(T);
		}
	}
}

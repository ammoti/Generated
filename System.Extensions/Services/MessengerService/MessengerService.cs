// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Concurrent;
	using System.Linq;

	public class MessengerService : IMessengerService
	{
		#region [ Members ]

		private readonly ConcurrentDictionary<MessengerKey, object> _actions = null;

		#endregion

		#region [ Constructor ]

		public MessengerService()
		{
			_actions = new ConcurrentDictionary<MessengerKey, object>();
		}

		#endregion

		#region [ IMessengerService Implementation ]

		/// <summary>
		/// Registers an action that is executed when Notify() is invoked.
		/// </summary>
		///
		/// <typeparam name="T">
		/// Type of the object.
		/// </typeparam>
		///
		/// <param name="key">
		/// Key of the messenger.
		/// </param>
		///
		/// <param name="action">
		/// Action to register.
		/// </param
		public void Register<T>(string key, Action<T> action)
		{
			_actions.TryAdd(new MessengerKey(typeof(T), key), (object)action);
		}

		/// <summary>
		/// Unregisters an action.
		/// </summary>
		///
		/// <typeparam name="T">
		/// Type of the object.
		/// </typeparam>
		///
		/// <param name="key">
		/// Key of the messenger.
		/// </param>
		/// 
		/// <param name="throwExceptionOnError">
		/// Value indicating whether an exception is thrown when an unregistration error occurred.
		/// </param>
		public void Unregister<T>(string key, bool throwExceptionOnError = true)
		{
			var t = typeof(T);
			var messengerEntry = _actions.Where(kvp => kvp.Key.Type == t && kvp.Key.Key == key).FirstOrDefault();

			if (messengerEntry.Key == null)
			{
				if (throwExceptionOnError)
				{
					ThrowException.Throw(
						"Cannot unregister (key = '{0}', Type = {1}) because it does not exist",
						key,
						t.FullName);
				}
			}

			object removedObj;

			_actions.TryRemove(messengerEntry.Key, out removedObj);

			removedObj = null;
		}

		/// <summary>
		/// Raises a notification. All registered actions associated to the key will be executed.
		/// </summary>
		///
		/// <typeparam name="T">
		/// Type of the object.
		/// </typeparam>
		///
		/// <param name="key">
		/// Key of the messenger.
		/// </param>
		///
		/// <param name="obj">
		/// Value.
		/// </param>
		public void Notify<T>(string key, T obj)
		{
			foreach (var action in _actions.Where(kvp => kvp.Key.Type == typeof(T) && kvp.Key.Key == key))
			{
				((Action<T>)action.Value)(obj);
			}
		}

		#endregion
	}
}

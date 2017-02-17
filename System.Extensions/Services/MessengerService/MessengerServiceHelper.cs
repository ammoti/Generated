// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Quick access to the IMessengerService defined in the configuration file (ServiceLocator definition).
	/// </summary>
	public class MessengerServiceHelper : Singleton<MessengerServiceHelper>, IMessengerService
	{
		#region [ Members ]

		private static readonly IMessengerService _messengerService = ServiceLocator.Current.Resolve<IMessengerService>();

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
		/// </param>
		public void Register<T>(string key, Action<T> action)
		{
			_messengerService.Register<T>(key, action);
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
			_messengerService.Unregister<T>(key, throwExceptionOnError);
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
			_messengerService.Notify(key, obj);
		}

		#endregion
	}
}

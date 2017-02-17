// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public interface IMessengerService
	{
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
		void Register<T>(string key, Action<T> action);

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
		void Unregister<T>(string key, bool throwExceptionOnError = true);

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
		void Notify<T>(string key, T obj);
	}
}

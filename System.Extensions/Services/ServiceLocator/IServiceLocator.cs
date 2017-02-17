// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public interface IServiceLocator
	{
		/// <summary>
		/// Resolves and gets an instance from the instance container given a unique name or the T type.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the instance. Must be an interface.
		/// </typeparam>
		/// 
		/// <param name="name">
		/// Unique name to identicate the named instance to retrieve.
		/// </param>
		/// 
		/// <returns>
		/// The requested instance. Returns null value if not found.
		/// </returns>
		T Resolve<T>(string name = null);

		/// <summary>
		/// Registers an instance in the container using a unique name.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the instance. Should be an interface.
		/// </typeparam>
		/// 
		/// <param name="name">
		/// Unique name to identicate the instance.
		/// </param>
		/// 
		/// <param name="instance">
		/// Instance to register.
		/// </param>
		/// 
		/// <param name="throwExceptionIfAlreadyRegistered">
		/// Indicates whether an exception is raised on name already used.
		/// </param>
		/// 
		/// <returns>
		/// The registered instance.
		/// </returns>
		T RegisterInstance<T>(string name, T instance, bool throwExceptionIfAlreadyRegistered = true);

		bool HasInstance<T>(string name = null);

		bool HasInstance<T>(string name, out T instance);

		bool HasInstance<T>(out T instance);
	}
}

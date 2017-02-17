// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// IoC implementation. Used to create, register and get instances from instance container.
	/// </summary>
	public class ServiceLocator : Singleton<ServiceLocator>, IServiceLocator, IDisposable
	{
		#region [ Members ]

		private readonly ConcurrentDictionary<string, object> _container = null;
		private readonly ILoggerService _loggerService = null;

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Default constructor. Uses &lt;serviceLocatorServiceConfiguration&gt; section from application configuration file.
		/// </summary>
		public ServiceLocator()
		{
			_container = new ConcurrentDictionary<string, object>();

			var section = ServiceLocatorSectionHandler.GetSection();
			if (section != null)
			{
				foreach (var entry in section.Entries.Values)
				{
					string key = entry.IsNamed ? entry.Name : entry.Interface;

					if (entry.InstancingMode == InstancingMode.Singleton)
					{
						Type instanceType = Type.GetType(entry.Implementation, true, true);
						object instance = Activator.CreateInstance(instanceType);

						if (!_container.ContainsKey(key))
						{
							_container.TryAdd(key, instance);
						}
					}
					else // InstancingMode.NewInstance
					{
						if (!_container.ContainsKey(key))
						{
							_container.TryAdd(key, entry.Implementation);
						}
					}

					if (_loggerService == null)
					{
						_loggerService = _container.Values.OfType<ILoggerService>().FirstOrDefault();						
					}

					if (_loggerService != null)
					{
						_loggerService.WriteLine(this, LogStatusEnum.Info, "Registered {0} -> {1} ", entry.InstancingMode, entry.Implementation);
					}
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~ServiceLocator()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ IServiceLocator Implementation ]

		/// <summary>
		/// Gets an instance from the instance container given a unique name or the T type.
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
		public T Resolve<T>(string name = null)
		{
			T instance = default(T);

			if (this.HasInstance<T>(name, out instance))
			{
				return instance;
			}

			if (_loggerService != null)
			{
				MethodBase mb = new StackFrame(1).GetMethod();
				string from = string.Format("module = '{0}', callerFullName  = '{1}.{2}()'", mb.Module.Name, mb.ReflectedType, mb.Name);

				if (!string.IsNullOrEmpty(name))
				{
					_loggerService.WriteLine(this, LogStatusEnum.Error,
						"Fails to get instance -> name = '{0}' ({1})", name, from);
				}
				else
				{
					_loggerService.WriteLine(this, LogStatusEnum.Error,
						"Fails to get instance -> type = '{0}' ({1})", typeof(T).FullName, from);
				}
			}

			return default(T);
		}

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
		public T RegisterInstance<T>(string name, T instance, bool throwExceptionIfAlreadyRegistered = true)
		{
			if (string.IsNullOrEmpty(name))
			{
				ThrowException.ThrowArgumentNullException("name");
			}

			T registeredInstance;

			if (this.HasInstance<T>(name, out registeredInstance))
			{
				if (throwExceptionIfAlreadyRegistered)
				{
					ThrowException.ThrowInvalidOperationException(string.Format(
						"The instance associated to the '{0}' name is already registered", name));
				}

				return registeredInstance;
			}
			else
			{
				_container.TryAdd(name, instance);
			}

			return instance;
		}

		public bool HasInstance<T>(string name = null)
		{
			T instance;
			return this.HasInstance<T>(name, out instance);
		}

		public bool HasInstance<T>(string name, out T instance)
		{
			instance = default(T);

			if (!string.IsNullOrEmpty(name))
			{
				if (_container.ContainsKey(name))
				{
					if (_container[name] is string)
					{
						Type instanceType = Type.GetType(_container[name].ToString(), true, true);
						instance = (T)Activator.CreateInstance(instanceType);
					}
					else
					{
						instance = (T)_container[name];
					}

					return true;
				}
			}
			else
			{
				string sInterface = string.Format("{0}, {1}", typeof(T).FullName, (typeof(T)).Assembly.FullName.Split(',').First());
				if (_container.ContainsKey(sInterface))
				{
					if (_container[sInterface] is string)
					{
						Type instanceType = Type.GetType(_container[sInterface].ToString(), throwOnError: true, ignoreCase: true);
						instance = (T)Activator.CreateInstance(instanceType);
					}
					else
					{
						instance = (T)_container[sInterface];
					}

					return true;
				}
			}

			return false;
		}

		public bool HasInstance<T>(out T instance)
		{
			return this.HasInstance<T>(null, out instance);
		}

		#endregion

		#region [ CreateInstance Methods ]

		/// <summary>
		/// Creates an instance. When using InstancingMode.Singleton the instance is registered in the instance container for next uses.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the instance. Should be a concrete class.
		/// </typeparam>
		/// 
		/// <param name="instancingMode">
		/// Instancing mode enumeration.
		/// </param>
		/// 
		/// <param name="parameters">
		/// An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If args is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
		/// </param>
		/// 
		/// <returns>
		/// The created instance.
		/// </returns>
		public T CreateInstance<T>(InstancingMode instancingMode, params object[] parameters) where T : class, new()
		{
			object instance = null;

			if (instancingMode == InstancingMode.Singleton)
			{
				string key = string.Format("{0}.{1}", typeof(T).Namespace, typeof(T).Name);

				if (!_container.ContainsKey(key))
				{
					instance = (T)Activator.CreateInstance(typeof(T), parameters, null);
					_container.TryAdd(key, instance);
				}
				else
				{
					instance = (T)_container[key];
				}
			}

			if (instancingMode == InstancingMode.NewInstance)
			{
				instance = (T)Activator.CreateInstance(typeof(T), null, null);
			}

			return (T)instance;
		}

		/// <summary>
		/// Creates an instance using assembly qualified name and instancing mode. When using InstancingMode.Singleton the instance is registered in the instance container for next uses.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the instance. Should be an interface.
		/// </typeparam>
		/// 
		/// <param name="asssemblyQualifiedName">
		/// Assembly qualified name ("Namespace.TypeName, AssemblyName").
		/// </param>
		/// 
		/// <param name="instancingMode">
		/// Instancing mode enumeration.
		/// </param>
		/// 
		/// <param name="parameters">
		/// An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If args is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
		/// </param>
		/// 
		/// <returns>
		/// The created instance.
		/// </returns>
		public T CreateInstance<T>(string asssemblyQualifiedName, InstancingMode instancingMode, params object[] parameters)
		{
			object instance = null;
			Type instanceType = Type.GetType(asssemblyQualifiedName, true);

			if (instancingMode == InstancingMode.Singleton)
			{
				if (!_container.ContainsKey(asssemblyQualifiedName))
				{
					instance = Activator.CreateInstance(instanceType, parameters);
					_container.TryAdd(asssemblyQualifiedName, instance);
				}
				else
				{
					instance = _container[asssemblyQualifiedName];
				}
			}

			if (instancingMode == InstancingMode.NewInstance)
			{
				instance = Activator.CreateInstance(instanceType, parameters);
			}

			return (T)instance;
		}

		/// <summary>
		/// Creates an instance using instance type and instancing mode. When using InstancingMode.Singleton the instance is registered in the instance container for next uses.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the instance. Should be an interface.
		/// </typeparam>
		/// 
		/// <param name="instanceType">
		/// Instance type to create.
		/// </param>
		/// 
		/// <param name="instancingMode">
		/// Instancing mode enumeration.
		/// </param>
		/// 
		/// <param name="parameters">
		/// An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If args is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
		/// </param>
		/// 
		/// <returns>
		/// The created instance.
		/// </returns>
		public T CreateInstance<T>(Type instanceType, InstancingMode instancingMode, params object[] parameters)
		{
			object instance = null;

			if (instancingMode == InstancingMode.Singleton)
			{
				if (!_container.ContainsKey(instanceType.FullName))
				{
					instance = Activator.CreateInstance(instanceType, parameters);
					_container.TryAdd(instanceType.FullName, instance);
				}
				else
				{
					instance = _container[instanceType.FullName];
				}
			}

			if (instancingMode == InstancingMode.NewInstance)
			{
				instance = Activator.CreateInstance(instanceType, parameters);
			}

			return (T)instance;
		}

		#endregion

		#region [ IDisposable Implementation ]

		private bool _isDisposed = false;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// 
		/// <param name="disposing">
		/// For internal use.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				lock (_locker)
				{
					if (!_isDisposed)
					{
						var iterator = _container.Values.GetEnumerator();

						while (iterator.MoveNext())
						{
							var disposer = iterator.Current as IDisposable;
							disposer.SafeDispose();
						}

						_isDisposed = true;
					}
				}
			}
		}

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.ClientCore
{
	using System;
	using System.Configuration;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Configuration;
	using System.ServiceModel.Description;

	using VahapYigit.Test.Core;

	/// <summary>
	/// Class used to create WCF proxies on client-side.
	/// </summary>
	public static class ServiceProxy
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		static ServiceProxy()
		{
			ClientCredentials = new ClientCredentials();
		}

		/// <summary>
		/// Sets the client credentials.
		/// </summary>
		/// 
		/// <param name="identifier">
		/// Identifier.
		/// </param>
		/// 
		/// <param name="password">
		/// Password.
		/// </param>
		public static void SetClientCredentials(string identifier, string password)
		{
			ClientCredentials.UserName.UserName = identifier;
			ClientCredentials.UserName.Password = password;
		}

		/// <summary>
		/// Sets the client credentials using an user context.
		/// </summary>
		/// 
		/// <param name="userContext">
		/// User context.
		/// </param>
		public static void SetClientCredentials(IUserContext userContext)
		{
			if (userContext != null)
			{
				SetClientCredentials(userContext.Identifier, userContext.Password);
			}
		}

		/// <summary>
		/// Gets the client credentials.
		/// </summary>
		internal static ClientCredentials ClientCredentials { get; private set; }
	}

	/// <summary>
	/// Class used to create WCF proxies on client-side.
	/// Using the &lt;system.serviceModel&gt; section from App.config file.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// Contract of the service.
	/// </typeparam>
	public class ServiceProxy<T> : IDisposable
		where T : class
	{
		#region [ Members ]

		private static bool? _isLocal = null;

		private static T _channel = default(T);
		private static ChannelFactory<T> _factory = null;

		private readonly static object _locker = new object();

		#endregion

		#region [ Properties ]

		private string ServiceProxyName
		{
			get { return string.Format("ServiceProxy<{0}>", typeof(T).Name); }
		}

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ServiceProxy()
		{
			if (_isLocal == null)
			{
				lock (_locker)
				{
					if (_isLocal == null)
					{
						ServiceProxySection cfgSection = ServiceProxySectionHandler.GetSection();
						if (cfgSection != null)
						{
							_isLocal = cfgSection.IsLocal;
						}
						else
						{
							System.Diagnostics.Debugger.Break(); // You can comment this line.

							// Cannot read the serviceProxyConfiguration section from the configuration file.
							// -> We assume that we are using local instances.

							_isLocal = true;

							// You can also choose to throw an exception...

							//ThrowException.ThrowConfigurationErrorsException("Cannot read the serviceProxyConfiguration section from the configuration file");
						}
					}
				}
			}

			if (_isLocal == false)
			{
				if (_channel == null)
				{
					lock (_locker)
					{
						if (_channel == null)
						{
							ClientSection clientSection =
								ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

							foreach (ChannelEndpointElement endpoint in clientSection.Endpoints)
							{
								if (endpoint.Contract == typeof(T).FullName)
								{
									_factory = new ChannelFactory<T>(endpoint.Name);

									_factory.Credentials.UserName.UserName = ServiceProxy.ClientCredentials.UserName.UserName;
									_factory.Credentials.UserName.Password = ServiceProxy.ClientCredentials.UserName.Password;

									LoggerServiceHelper.Current.WriteLine(ServiceProxyName, LogStatusEnum.Info,
										"ServiceProxy<{0}> Registering Proxy -> {1}", typeof(T).Name, typeof(T).FullName);

									_channel = _factory.CreateChannel();

									break;
								}
							}

							if (_channel == default(T))
							{
								Type t = typeof(T);

								string error = string.Format(
									"ServiceProxy<{0}> Fails Registering Proxy -> {1} (no endpoint found in the application configuration file)",
									t.Name,
									t.FullName);

								LoggerServiceHelper.Current.WriteLine(ServiceProxyName, LogStatusEnum.Error, error);

								ThrowException.Throw(error);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~ServiceProxy()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ Public Properties ]

		/// <summary>
		/// Gets the service proxy (WCF or local - depends on the configuration).
		/// </summary>
		public T Proxy
		{
			get
			{
				if (_isLocal == true)
				{
					if (_channel == null)
					{
						lock (_locker)
						{
							if (_channel == null)
							{
								Type serviceType = typeof(T);

								string qualifiedName = serviceType.AssemblyQualifiedName;
								string serviceName = serviceType.Name.Substring(1);

								qualifiedName = qualifiedName.Replace(".Contracts", ".Services");       // Ugly hack...
								qualifiedName = qualifiedName.Replace(serviceType.Name, serviceName);   // Ugly hack...

								_channel = ServiceLocator.Current.CreateInstance<T>(qualifiedName, InstancingMode.NewInstance);
							}
						}
					}
				}
				else
				{
					if (((IChannel)_channel).State == CommunicationState.Faulted)
					{
						this.ResetProxy();
					}
				}

				return _channel;
			}
		}

		#endregion

		#region [ Private Methods ]

		/// <summary>
		/// Resets the proxy.
		/// </summary>
		private void ResetProxy()
		{
			LoggerServiceHelper.Current.WriteLine(ServiceProxyName, LogStatusEnum.Info,
				"ServiceProxy<{0}> Resetting Proxy -> {1}", typeof(T).Name, typeof(T).FullName);

			try
			{
				((IChannel)_channel).Close();
			}
			catch
			{
				try
				{
					((IChannel)_channel).Abort();
				}
				catch { }
			}

			_channel = _factory.CreateChannel();
		}

		#endregion

		public override string ToString()
		{
			if (_factory != null && _factory.Endpoint != null && _factory.Endpoint.Address != null)
			{
				return string.Format("{0} (binding = {1})", _factory.Endpoint.Address, _factory.Endpoint.Binding.Name);
			}

			return base.ToString();
		}

		#region [ IDisposable Implementation ]

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
			if (disposing)
			{
			}
		}

		#endregion
	}
}
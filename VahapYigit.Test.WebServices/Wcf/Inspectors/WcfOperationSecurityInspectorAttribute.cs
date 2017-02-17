// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Reflection;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;

	using VahapYigit.Test.Core;
	using VahapYigit.Test.Models;
	using VahapYigit.Test.Services;

	public class WcfOperationSecurityInspectorAttribute : Attribute, IParameterInspector, IServiceBehavior
	{
		#region [ Members ]

		private static readonly object _locker = null;

		private static readonly WcfOperationSecuritySection _configSection = null;

		#endregion

		#region [ Constructor ]

		static WcfOperationSecurityInspectorAttribute()
		{
			_locker = new object();

			_configSection = WcfOperationSecuritySectionHandler.GetSection();
		}

		#endregion

		#region [ IParameterInspector Implementation ]

		public object BeforeCall(string operationName, object[] inputs)
		{
			// Get the UserContext...

			IUserContext userContext = GetUserContext(inputs);
			if (userContext == null)
			{
				this.ThrowSecurityException(
					"WCF operations cannot be invoked without IUserContext instance!");
			}

			if (!_configSection.IsEnabled)
			{
				return null;
			}

			var mi = WcfHelper.GetInvokedMethodInfoFromOperationContext();
			if (mi != null)
			{
				// Retrieve the method required roles (from cache first)...

				string key = string.Format("WcfOperationSecurityInspectorAttribute.{0}.{1}.RequiredRoles",
					mi.DeclaringType.FullName, operationName);

				IList<string> requiredRoles = CacheServiceHelper.Current.Get<List<string>>(key);
				if (requiredRoles == null)
				{
					lock (_locker)
					{
						requiredRoles = CacheServiceHelper.Current.Get<List<string>>(key);
						if (requiredRoles == null)
						{
							requiredRoles = new List<string>();

							// Using Reflection to get the required roles...

							OperationSecurityAttribute attr = mi.GetCustomAttribute<OperationSecurityAttribute>();
							if (attr != null)
							{
								requiredRoles = attr.Roles;
							}

							CacheServiceHelper.Current.Add(key, requiredRoles, TimeSpan.FromHours(1));
						}
					}
				}

				if (requiredRoles.Count() == 0) // No specific role required -> exit.
				{
					return null;
				}

				// Retrieve the User roles (from cache first)...

				key = string.Format("WcfOperationSecurityInspectorAttribute.UserRoles.{0}", userContext.Identifier);

				IList<string> userRoles = CacheServiceHelper.Current.Get<List<string>>(key);
				if (userRoles == null)
				{
					lock (_locker)
					{
						userRoles = CacheServiceHelper.Current.Get<List<string>>(key);
						if (userRoles == null)
						{
							// Write your custom code here to retrieve the User roles...
							// ------------------------------------------------------------

							User user = null;
							if (ServiceContext.AuthenticationService.IsRegistered(userContext, out user))
							{
								userRoles = user.UserRoleCollection.Select(i => i.Role).Select(j => j.CodeRef).ToList();

								CacheServiceHelper.Current.Add(key, userRoles, TimeSpan.FromHours(1));
							}
							else
							{
								this.ThrowSecurityException(
									"To execute the '{0}.{1}' operation the user '{2}' must be authenticated",
									mi.DeclaringType.FullName, mi.Name, userContext.Identifier);
							}

							// ------------------------------------------------------------
						}
					}
				}

				// Check if the WCF operation can be invoked by the User...

				bool hasRole = false;

				foreach (string role in userRoles)
				{
					if (requiredRoles.Any(i => i == role))
					{
						hasRole = true;
						break;
					}
				}

				if (!hasRole)
				{
					this.ThrowSecurityException(
						"To execute the '{0}.{1}' operation the user '{2}' must have at least one on the following roles: '{3}'",
						mi.DeclaringType.FullName, mi.Name, userContext.Identifier, string.Join(", ", requiredRoles));
				}
			}

			return null;
		}

		public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
		{
		}

		#endregion

		#region [ IServiceBehavior Implementation ]

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
			{
				foreach (var endPoint in channelDispatcher.Endpoints)
				{
					foreach (var opertaion in endPoint.DispatchRuntime.Operations)
					{
						opertaion.ParameterInspectors.Add(this);
					}
				}
			}
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		#endregion

		#region [ Private Methods ]

		private void ThrowSecurityException(string message, params object[] args)
		{
			message = string.Format(message, args);

			LoggerServiceHelper.Current.WriteLine(this, LogStatusEnum.Warning, message);

			ThrowException.ThrowSecurityException(message);
		}

		private static IUserContext GetUserContext(object[] inputs)
		{
			IUserContext userContext = null;

			foreach (object input in inputs)
			{
				if (input is IUserContext)
				{
					userContext = (IUserContext)input;
					break;
				}
			}

			return userContext;
		}

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;

	public sealed class OperationExceptionBehaviorAttribute : Attribute, IServiceBehavior, IEndpointBehavior, IContractBehavior
	{
		#region [ IContractBehavior Implementation ]

		void IContractBehavior.AddBindingParameters(ContractDescription contract, ServiceEndpoint endpoint, BindingParameterCollection parameters)
		{
		}

		void IContractBehavior.ApplyClientBehavior(ContractDescription contract, ServiceEndpoint endpoint, ClientRuntime runtime)
		{
			Debug.WriteLine(string.Format("Applying client OperationExceptionBehaviorAttribute to contract {0}", contract.ContractType));
			this.ApplyClientBehavior(runtime);
		}

		void IContractBehavior.ApplyDispatchBehavior(ContractDescription contract, ServiceEndpoint endpoint, DispatchRuntime runtime)
		{
			Debug.WriteLine(string.Format("Applying dispatch OperationExceptionBehaviorAttribute to contract {0}", contract.ContractType.FullName));
			this.ApplyDispatchBehavior(runtime.ChannelDispatcher);
		}

		void IContractBehavior.Validate(ContractDescription contract, ServiceEndpoint endpoint)
		{
		}

		#endregion

		#region [ IEndpointBehavior Implementation ]

		void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection parameters)
		{
		}

		void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime runtime)
		{
			Debug.WriteLine(string.Format("Applying client OperationExceptionBehaviorAttribute to endpoint {0}", endpoint.Address));
			this.ApplyClientBehavior(runtime);
		}

		void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher dispatcher)
		{
			Debug.WriteLine(string.Format("Applying dispatch OperationExceptionBehaviorAttribute to endpoint {0}", endpoint.Address));
			this.ApplyDispatchBehavior(dispatcher.ChannelDispatcher);
		}

		void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
		{
		}

		#endregion

		#region [ IServiceBehavior Implementation ]

		void IServiceBehavior.AddBindingParameters(ServiceDescription service, ServiceHostBase host, Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
		{
		}

		void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription service, ServiceHostBase host)
		{
			Debug.WriteLine(string.Format("Applying dispatch OperationExceptionBehaviorAttribute to service {0}", service.ServiceType.FullName));
			foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
			{
				this.ApplyDispatchBehavior(dispatcher);
			}
		}

		void IServiceBehavior.Validate(ServiceDescription service, ServiceHostBase host)
		{
		}

		#endregion

		#region [ Private Members ]

		private void ApplyClientBehavior(ClientRuntime runtime)
		{
			if (!runtime.MessageInspectors.Any(i => i is OperationExceptionMessageInspector))
			{
				runtime.MessageInspectors.Add(new OperationExceptionMessageInspector());
			}
		}

		private void ApplyDispatchBehavior(ChannelDispatcher dispatcher)
		{
			if (!dispatcher.ErrorHandlers.Any(i => i is OperationExceptionErrorHandler))
			{
				dispatcher.ErrorHandlers.Add(new OperationExceptionErrorHandler());
			}
		}

		#endregion
	}
}

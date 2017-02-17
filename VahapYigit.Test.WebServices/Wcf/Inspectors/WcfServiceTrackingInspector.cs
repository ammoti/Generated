// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices
{
	using System;
	using System.Collections.ObjectModel;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;

	using VahapYigit.Test.Core;

	public class WcfServiceTrackingInspector : IDispatchMessageInspector, IServiceBehavior
	{
		#region [ Members ]

		private static readonly WcfServiceTrackingSection _configSection = null;

		#endregion

		#region [ Constructors ]

		static WcfServiceTrackingInspector()
		{
			_configSection = WcfServiceTrackingSectionHandler.GetSection();
		}

		#endregion

		#region [ IDispatchMessageInspector Implementation ]

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
		}

		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			TraceOperation(ref request, channel, instanceContext);
			return null;
		}

		#endregion

		#region [ IServiceBehavior Implementation ]

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
			{
				foreach (var endpoint in dispatcher.Endpoints)
				{
					endpoint.DispatchRuntime.MessageInspectors.Add(new WcfServiceTrackingInspector());
				}
			}
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		#endregion

		#region [ Private Methods ]

		private static void TraceOperation(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			if (_configSection.IsEnabled)
			{
				if (!request.IsEmpty)
				{
					// Enter your custom code in this block...

					string to = OperationContext.Current.IncomingMessageHeaders.To.ToString();
					string action = OperationContext.Current.IncomingMessageHeaders.Action;
					string operationName = action.Substring(action.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);

					string logLine = string.Format(
						"Request from {0} -> {1} (operationName = {2})",
						WcfHelper.GetClientIpAddress(), to, operationName);

					if (_configSection.WithMessageLogging)
					{
						logLine = string.Concat(
							logLine, Environment.NewLine, WcfHelper.MessageToString(ref request));
					}

					LoggerServiceHelper.Current.WriteLine("WcfServiceTracking", LogStatusEnum.Info, logLine);
				}
			}
		}

		#endregion
	}
}

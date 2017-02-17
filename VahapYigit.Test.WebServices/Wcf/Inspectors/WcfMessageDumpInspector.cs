// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices
{
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;

	public class WcfMessageDumpInspector : IDispatchMessageInspector, IServiceBehavior
	{
		#region [ Direction Enum ]

		enum Direction
		{
			Incoming,
			Outgoing
		}

		#endregion

		#region [ Constructor ]

		public WcfMessageDumpInspector()
		{
		}

		#endregion

		#region [ IDispatchMessageInspector Implementation ]

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
			DumpMessage(ref reply, Direction.Outgoing);
		}

		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			DumpMessage(ref request, Direction.Incoming);
			return null;
		}

		#endregion

		#region [ IServiceBehavior ]

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
			{
				foreach (var endpoint in dispatcher.Endpoints)
				{
					endpoint.DispatchRuntime.MessageInspectors.Add(new WcfMessageDumpInspector());
				}
			}
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		#endregion

		#region [ Private Methods ]

		private static void DumpMessage(ref Message message, Direction direction)
		{
			// Debug, Logger, etc...

			Debug.WriteLine("WcfMessageDumpInspector({0})", direction);
			Debug.WriteLine("---------------------------------------");
			Debug.WriteLine(WcfHelper.MessageToString(ref message));
			Debug.WriteLine("---------------------------------------");
		}

		#endregion
	}
}
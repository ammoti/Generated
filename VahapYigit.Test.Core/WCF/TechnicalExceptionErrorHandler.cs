// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Diagnostics;
	using System.Runtime.Serialization;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Dispatcher;

	public class TechnicalExceptionErrorHandler : IErrorHandler
	{
		#region [ IErrorHandler Implementation ]

		public bool HandleError(Exception error)
		{
			return !(error is FaultException) &&
				   !(error is OperationException) &&
				   !(error is EntityValidationException);
		}

		public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
		{
			// To see the client's request : OperationContext.Current.RequestContext

			if (!(error is FaultException) &&
				!(error is OperationException) &&
				!(error is EntityValidationException))
			{
				LoggerServiceHelper.Current.TraceException(this, error);

				MessageFault messageFault = MessageFault.CreateFault(
					new FaultCode("Sender"),
					new FaultReason(error.Message),
					error,
					new NetDataContractSerializer());

				fault = Message.CreateMessage(version, messageFault, null);
			}
		}

		#endregion
	}
}

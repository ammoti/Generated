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

	public class OperationExceptionErrorHandler : IErrorHandler
	{
		#region [ IErrorHandler Implementation ]

		public bool HandleError(Exception error)
		{
			return error is OperationException;
		}

		public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
		{
			if (error is OperationException)
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

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.IO;
	using System.ServiceModel;
	using System.Runtime.Serialization;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Dispatcher;
	using System.Xml;

	public class TechnicalExceptionMessageInspector : IClientMessageInspector
	{
		#region [ IClientMessageInspector Implementation ]

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
			if (reply.IsFault)
			{
				MessageBuffer buffer = reply.CreateBufferedCopy(int.MaxValue);
				Message copy = buffer.CreateMessage();

				reply = buffer.CreateMessage();

				object faultDetail = ClientMessageInspectorHelper.ReadFaultDetail(copy);
				Exception exception = faultDetail as Exception;

				if (exception != null &&
					!(exception is OperationException) &&
					!(exception is EntityValidationException))
				{
					throw new TechnicalException(
						string.Format("{0}: {1}", exception.GetType().FullName, exception.Message),
						exception);
				}
			}
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			return null; // Nothing to do.
		}

		#endregion
	}
}

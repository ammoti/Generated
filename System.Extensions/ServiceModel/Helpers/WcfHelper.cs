// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.ServiceModel
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization.Json;
	using System.ServiceModel.Channels;
	using System.Text;
	using System.Xml;

	public static class WcfHelper
	{
		/// <summary>
		/// Gets the client IP address from the WCF context (OperationContext).
		/// </summary>
		/// 
		/// <returns>
		/// The client IP address.
		/// </returns>
		public static string GetClientIpAddress()
		{
			MessageProperties props = OperationContext.Current.IncomingMessageProperties;
			RemoteEndpointMessageProperty endpoint = props[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

			return endpoint.Address;
		}

		/// <summary>
		/// Gets the Invoked MethodInfo from the current OperationContext.
		/// </summary>
		/// 
		/// <returns>
		/// The MethodInfo object.
		/// </returns>
		public static MethodInfo GetInvokedMethodInfoFromOperationContext()
		{
			string methodName;

			OperationContext operationContext = OperationContext.Current;
			string bindingName = operationContext.EndpointDispatcher.ChannelDispatcher.BindingName;

			if (bindingName.ToUpperInvariant().Contains("WEBHTTPBINDING"))
			{
				if (System.Diagnostics.Debugger.IsAttached) // REST not tested!
					System.Diagnostics.Debugger.Break();

				// REST request 
				methodName = (string)operationContext.IncomingMessageProperties["HttpOperationName"];
			}
			else
			{
				// SOAP request 
				string action = operationContext.IncomingMessageHeaders.Action;
				methodName = operationContext.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o => o.Action == action).Name;
			}

			Type hostType = operationContext.Host.Description.ServiceType;

			return hostType.GetMethod(methodName);
		}

		public static string MessageToString(ref Message message)
		{
			WebContentFormat messageFormat = message.GetContentFormat();

			MemoryStream ms = new MemoryStream();
			XmlDictionaryWriter writer = null;

			switch (messageFormat)
			{
				case WebContentFormat.Default:
				case WebContentFormat.Xml:
					writer = XmlDictionaryWriter.CreateTextWriter(ms);
					break;
				case WebContentFormat.Json:
					writer = JsonReaderWriterFactory.CreateJsonWriter(ms);
					break;
				case WebContentFormat.Raw:
					return ReadRawBody(ref message);
			}

			message.WriteMessage(writer);

			writer.Flush();

			string messageBody = Encoding.UTF8.GetString(ms.ToArray());

			// The messageBody can be modified here...

			// Then recreate the message...

			ms.Position = 0;

			// if the message body was modified, needs to reencode it, as show below
			// ms = new MemoryStream(Encoding.UTF8.GetBytes(messageBody));

			XmlDictionaryReader reader;
			if (messageFormat == WebContentFormat.Json)
			{
				reader = JsonReaderWriterFactory.CreateJsonReader(ms, XmlDictionaryReaderQuotas.Max);
			}
			else
			{
				reader = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);
			}

			var newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
			newMessage.Properties.CopyProperties(message.Properties);

			message = newMessage;

			return messageBody;
		}

		public static string ReadRawBody(ref Message message)
		{
			XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
			bodyReader.ReadStartElement("Binary");

			byte[] bodyBytes = bodyReader.ReadContentAsBase64();
			string messageBody = Encoding.UTF8.GetString(bodyBytes);

			// Recreate the message...

			var ms = new MemoryStream(); // Closed/Disposed by XmlDictionaryWriter.

			using (var writer = XmlDictionaryWriter.CreateBinaryWriter(ms))
			{
				writer.WriteStartElement("Binary");
				writer.WriteBase64(bodyBytes, 0, bodyBytes.Length);
				writer.WriteEndElement();
				writer.Flush();

				ms.Position = 0;

				var reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
				var newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);

				newMessage.Properties.CopyProperties(message.Properties);
				message = newMessage;

				return messageBody;
			}
		}
	}
}

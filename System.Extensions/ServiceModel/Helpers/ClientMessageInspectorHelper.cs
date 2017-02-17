// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

// From http://stackoverflow.com/questions/10916645/communicationexception-wcf

namespace System.ServiceModel
{
	using System.IO;
	using System.Runtime.Serialization;
	using System.ServiceModel.Channels;
	using System.Xml;

	public static class ClientMessageInspectorHelper
	{
		public static object ReadFaultDetail(Message reply)
		{
			const string detailElementName = "detail";

			using (var reader = reply.GetReaderAtBodyContents())
			{
				// Find <soap:Detail>
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.LocalName.ToLower() == detailElementName)
					{
						break;
					}
				}

				// Did we find it?
				if (reader.NodeType != XmlNodeType.Element || reader.LocalName.ToLower() != detailElementName)
				{
					return null;
				}

				// Move to the contents of <soap:Detail>
				if (!reader.Read())
				{
					return null;
				}

				// Deserialize the fault
				NetDataContractSerializer serializer = new NetDataContractSerializer();

				try
				{
					return serializer.ReadObject(reader);
				}
				catch (FileNotFoundException)
				{
					// Serializer was unable to find assembly where exception is defined 
					return null;
				}
			}
		}
	}
}

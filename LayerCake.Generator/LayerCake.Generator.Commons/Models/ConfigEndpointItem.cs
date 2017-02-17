// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System.Xml;

	[DoNotPrune]
	[DoNotObfuscate]
	public class ConfigEndpointItem
	{
		#region [ Constructor ]

		public ConfigEndpointItem(XmlNode endpointNode)
		{
			if (endpointNode.Attributes["name"] != null)
				this.Name = endpointNode.Attributes["name"].InnerText;

			if (endpointNode.Attributes["address"] != null)
				this.Address = endpointNode.Attributes["address"].InnerText;

			if (endpointNode.Attributes["binding"] != null)
				this.Binding = endpointNode.Attributes["binding"].InnerText;

			if (endpointNode.Attributes["bindingConfiguration"] != null)
				this.BindingConfiguration = endpointNode.Attributes["bindingConfiguration"].InnerText;

			if (endpointNode.Attributes["behaviorConfiguration"] != null)
				this.BehaviorConfiguration = endpointNode.Attributes["behaviorConfiguration"].InnerText;

			if (endpointNode.Attributes["contract"] != null)
				this.Contract = endpointNode.Attributes["contract"].InnerText;
		}

		#endregion

		#region [ Properties ]

		public string Name { get; private set; }

		public string Address { get; private set; }

		public string Binding { get; private set; }

		public string BindingConfiguration { get; private set; }

		public string BehaviorConfiguration { get; private set; }

		public string Contract { get; private set; }

		#endregion
	}
}

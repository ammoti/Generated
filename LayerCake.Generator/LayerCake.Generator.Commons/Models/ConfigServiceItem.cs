// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System.Collections.Generic;
    using System.Xml;

	[DoNotPrune]
	[DoNotObfuscate]
	public class ConfigServiceItem
	{
		#region [ Constructor ]

		public ConfigServiceItem()
		{
            this.CustomEndpoints = new List<ConfigEndpointItem>();
		}

		public ConfigServiceItem(XmlNode serviceNode)
			: this()
		{
			if (serviceNode.Attributes["name"] != null)
				this.Name = serviceNode.Attributes["name"].InnerText;

			if (serviceNode.Attributes["behaviorConfiguration"] != null)
				this.BehaviorConfiguration = serviceNode.Attributes["behaviorConfiguration"].InnerText;

			foreach (XmlNode endpointNode in serviceNode.ChildNodes)
			{
				if (endpointNode.Name == "endpoint")
				{
                    this.CustomEndpoints.Add(new ConfigEndpointItem(endpointNode));
				}
			}
		}

		#endregion

		#region [ Properties ]

		public string Name { get; private set; }

		public string BehaviorConfiguration { get; private set; }

		public IList<ConfigEndpointItem> CustomEndpoints { get; private set; }

		#endregion
	}
}

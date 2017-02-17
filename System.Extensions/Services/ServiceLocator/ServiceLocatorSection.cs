// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Xml;

	public class InstanceEntry
	{
		public string Name { get; set; }
		public string Interface { get; set; }
		public string Implementation { get; set; }
		public InstancingMode InstancingMode { get; set; }

		public bool IsNamed { get { return !string.IsNullOrEmpty(this.Name); } }
	}

	/// <summary>
	/// ServiceLocatorSection class.
	/// </summary>
	public class ServiceLocatorSection
	{
		private readonly IDictionary<string, InstanceEntry> _instanceEntries = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ServiceLocatorSection()
		{
			_instanceEntries = new Dictionary<string, InstanceEntry>();
		}

		/// <summary>
		/// Loads the serviceLocatorConfiguration Xml section.
		/// </summary>
		/// <param name="sectionNode">The serviceLocatorConfiguration root node.</param>
		internal void Load(XmlNode sectionNode)
		{
			try
			{
				int index = 0;
				foreach (XmlNode entryNode in sectionNode.FirstChild.ChildNodes)
				{
					if (entryNode.NodeType != XmlNodeType.Element)
					{
						continue;
					}

					if (entryNode.Attributes["name"] == null && entryNode.Attributes["interface"] == null)
					{
						ThrowException.ThrowConfigurationErrorsException(
							string.Format("The attribute 'name' or 'interface' is required (block #{0})", index));
					}

					if (entryNode.Attributes["implementation"] == null)
					{
						ThrowException.ThrowConfigurationErrorsException(
							string.Format("The attribute 'implementation' is required (block #{0})", index));
					}

					if (entryNode.Attributes["instancingMode"] == null)
					{
						ThrowException.ThrowConfigurationErrorsException(
							string.Format("The attribute 'instancingMode' is required (block #{0})", index));
					}

					string sName = null;
					string sInterface = null;

					if (entryNode.Attributes["name"] != null)
					{
						sName = entryNode.Attributes["name"].Value;
						if (string.IsNullOrEmpty(sName))
						{
							ThrowException.ThrowConfigurationErrorsException(
								string.Format("The attribute 'name' requires a value (block #{0})", index));
						}
					}

					if (entryNode.Attributes["interface"] != null)
					{
						sInterface = entryNode.Attributes["interface"].Value;
						if (string.IsNullOrEmpty(sInterface))
						{
							ThrowException.ThrowConfigurationErrorsException(
								string.Format("The attribute 'interface' requires a value (block #{0})", index));
						}
					}

					string sImplementation = entryNode.Attributes["implementation"].Value;
					if (string.IsNullOrEmpty(sImplementation))
					{
						ThrowException.ThrowConfigurationErrorsException(
							string.Format("The attribute 'implementation' requires a value (block #{0})", index));
					}

					string sInstancingMode = entryNode.Attributes["instancingMode"].Value;
					if (string.IsNullOrEmpty(sInstancingMode))
					{
						ThrowException.ThrowConfigurationErrorsException(
							string.Format("The attribute 'instancingMode' requires a value (block #{0})", index));
					}

					if (sInstancingMode != "Singleton" && sInstancingMode != "NewInstance")
					{
						ThrowException.ThrowConfigurationErrorsException(
							string.Format("The attribute 'instancingMode' requires a value: 'Singelton' or 'NewInstance' (block #{0})", index));
					}

					InstancingMode instancingMode = (sInstancingMode == "Singleton") ?
						InstancingMode.Singleton :
						InstancingMode.NewInstance;

					if (_instanceEntries.ContainsKey(sName ?? sImplementation))
					{
						ThrowException.ThrowConfigurationErrorsException(
							string.Format("There is already a registered entry for '{0}' (block #{1})", sName ?? sImplementation, index));
					}

					_instanceEntries.Add(sName ?? sImplementation, new InstanceEntry
					{
						Name = sName,
						Interface = sInterface,
						Implementation = sImplementation,
						InstancingMode = instancingMode
					});

					index++;
				}
			}
			catch (System.Exception x)
			{
				ThrowException.ThrowConfigurationErrorsException(x.Message);
			}
		}

		public IDictionary<string, InstanceEntry> Entries
		{
			get { return _instanceEntries; }
		}
	}
}

// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;

	[Serializable]
	[DataContract(IsReference = true)]
	public class SearchCriterias : ISearchCriterias
	{
		#region [ Members ]

		private IEnumerable<PropertyInfo> _properties = null;

		#endregion

		#region [ Constructor & Initialize ]

		protected SearchCriterias()
		{
			this.PagingOptions = new PagingOptions();

			this.Initialize();
		}

		private void Initialize()
		{
			if (_properties == null)
			{
				_properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && p.CanWrite);
				foreach (var prop in _properties)
				{
					DefaultValueAttribute attr = prop.GetCustomAttribute<DefaultValueAttribute>();
					if (attr != null)
					{
						prop.SetValue(this, attr.Value);
					}
				}
			}
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}

		#endregion

		#region [ ISearchCriterias Implementation ]

		public virtual IDictionary<string, object> ToParameters()
		{
			const string separator = ",";
			var parameters = new Dictionary<string, object>();

			foreach (PropertyInfo prop in _properties)
			{
				var attr = prop.GetCustomAttribute<CriteriaMemberAttribute>();
				if (attr != null)
				{
					object value = prop.GetValue(this);

					if (value is string)
					{
						if (attr.IsFullTextSearch)
						{
							value = SqlServerFullTextSearchHelper.CreateFullTextSearchExpression(value.ToString());
						}
					}
					else if (value is List<int> || value is IList<int>)
					{
						value = string.Join(separator, (IList<int>)value);
					}
					else if (value is List<long> || value is IList<long>)
					{
						value = string.Join(separator, (IList<long>)value);
					}
					else if (value is List<string> || value is IList<string>)
					{
						value = string.Join(separator, (IList<string>)value);
					}

					parameters.Add(attr.SqlParameterName, value);
				}
			}

			// Paging parameters

			parameters.Add("@PagingCurrentPage", this.PagingOptions.CurrentPage);
			parameters.Add("@PagingRecordsPerPage", this.PagingOptions.RecordsPerPage);

			return parameters;
		}

		#endregion

		#region [ Properties ]

		[DataMember]
		public PagingOptions PagingOptions { get; private set; }

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Concurrent;

	/// <summary>
	/// Attribute used to define a named parameter (that can be modified at runtime).
	/// This attribute can be used many times to add several paremeters.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	sealed public class ParameterAttribute : Attribute
	{
		#region [ Members ]

		private static readonly ConcurrentDictionary<string, object> _parameterCache = new ConcurrentDictionary<string, object>();

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Initializes a new instance of the System.ParameterAttribute class.
		/// </summary>
		/// 
		/// <param name="key">
		/// Key.
		/// </param>
		/// 
		/// <param name="value">
		/// Value.
		/// </param>
		public ParameterAttribute(string key, object value)
		{
			this.Key = key;

			if (!_parameterCache.ContainsKey(this.Key))
			{
				_parameterCache.TryAdd(this.Key, value);
			}
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the key of the ParameterAttribute.
		/// </summary>
		public string Key
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the value of the ParameterAttribute.
		/// </summary>
		public object Value
		{
			get
			{
				object value;
				_parameterCache.TryGetValue(this.Key, out value);

				return value;
			}
			set
			{
				_parameterCache[this.Key] = value;
			}
		}

		#endregion

		#region [ Methods ]

		public override string ToString()
		{
			return string.Format("{0} (Key = {1}, Value = {2})", base.ToString(), this.Key, this.Value);
		}

		#endregion
	}
}

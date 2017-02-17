// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;

	public class RequiredPropertyController
	{
		#region [ Members ]

		private readonly object _parent = null;

		private IDictionary<string, PropertyInfo> _requiredProperties = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Creates and initializes the RequiredPropertyController instance.
		/// </summary>
		/// 
		/// <param name="parent">
		/// Object to manage.
		/// </param>
		/// 
		/// <param name="func">
		/// Function indicating whether the property is required (optional).
		/// </param>
		public RequiredPropertyController(object parent, Func<PropertyInfo, bool> func = null)
		{
			if (parent == null)
			{
				ThrowException.ThrowArgumentNullException("parent");
			}

			_parent = parent;
			_requiredProperties = new Dictionary<string, PropertyInfo>();

			var props = _parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			if (func != null)
			{
				foreach (var prop in props)
				{
					if (func(prop))
					{
						_requiredProperties.Add(prop.Name, prop);
					}
				}
			}
			else
			{
				foreach (var prop in props.Where(p => RequiredPropertyController.HasRequiredAttribute(p)))
				{
					_requiredProperties.Add(prop.Name, prop);
				}
			}
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Determines whether the property has the [Required] or [DataMember(IsRequired = true)] attribute.
		/// </summary>
		/// 
		/// <param name="property">
		/// The property.
		/// </param>
		/// 
		/// <returns>
		/// True if the property has the [Required] or [DataMember(IsRequired = true)] attribute; otherwise, false.
		/// </returns>
		private static bool HasRequiredAttribute(PropertyInfo property)
		{
			if (property == null)
			{
				return false;
			}

			// [Required] -> .NET Framework
			if (property.GetCustomAttribute<RequiredAttribute>() != null)
			{
				return true;
			}

			// [RequiredProperty] -> System.Extensions
			if (property.GetCustomAttribute<RequiredPropertyAttribute>() != null)
			{
				return true;
			}

			// [DataMember(IsRequired = true)] -> WCF Framework
			var attr = property.GetCustomAttribute<DataMemberAttribute>();
			if (attr != null)
			{
				if (attr.IsRequired)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the property is flagged with the [Required] or [DataMember(IsRequired = true)] attribute.
		/// </summary>
		/// 
		/// <param name="propertyName">
		/// The property name.
		/// </param>
		/// 
		/// <returns>
		/// True if the property is flagged with the [Required] or [DataMember(IsRequired = true)] attribute; otherwise, false.
		/// </returns>
		public bool IsRequiredProperty(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return false;
			}

			return _requiredProperties.ContainsKey(propertyName);
		}

		/// <summary>
		/// Determines whether the property is flagged with the [Required] or [DataMember(IsRequired = true)] attribute.
		/// </summary>
		/// 
		/// <param name="property">
		/// The property reference.
		/// </param>
		/// 
		/// <returns>
		/// True if the property is flagged with the [Required] or [DataMember(IsRequired = true)] attribute; otherwise, false.
		/// </returns>
		public bool IsRequiredProperty(PropertyInfo property)
		{
			if (property == null)
			{
				return false;
			}

			return this.IsRequiredProperty(property.Name) && _requiredProperties[property.Name] == property;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Determines whether at least one required property is not set.
		/// </summary>
		public bool HasErrors
		{
			get
			{
				foreach (var prop in _requiredProperties.Values)
				{
					if (prop.GetValue(_parent) == null)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Returns the property names that are not set.
		/// </summary>
		/// 
		/// <returns>
		/// List of the property names.
		/// </returns>
		public IEnumerable<string> GetErrors()
		{
			IList<string> errors = new List<string>();

			foreach (var prop in _requiredProperties.Values)
			{
				if (prop.GetValue(_parent) == null)
				{
					errors.Add(prop.Name);
				}
			}

			return errors;
		}

		#endregion
	}
}

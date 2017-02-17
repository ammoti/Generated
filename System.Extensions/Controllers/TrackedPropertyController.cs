// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class TrackedPropertyController
	{
		#region [ Members ]

		private IDictionary<string, PropertyInfo> _trackedProperties = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Creates and initializes the TrackedPropertyController instance.
		/// </summary>
		/// 
		/// <param name="parent">
		/// Object to manage.
		/// </param>
		public TrackedPropertyController(object parent)
		{
			if (parent == null)
			{
				ThrowException.ThrowArgumentNullException("parent");
			}

			_trackedProperties = new Dictionary<string, PropertyInfo>();

			foreach (var prop in parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
												 .Where(p => TrackedPropertyController.HasTrackedPropertyAttribute(p)))
			{
				_trackedProperties.Add(prop.Name, prop);
			}
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Determines whether the property has the TrackedPropertyAttribute attribute.
		/// </summary>
		/// 
		/// <param name="property">
		/// The property.
		/// </param>
		/// 
		/// <returns>
		/// True if the property has the TrackedPropertyAttribute attribute; otherwise, false.
		/// </returns>
		private static bool HasTrackedPropertyAttribute(PropertyInfo property)
		{
			if (property == null)
			{
				return false;
			}

			return property.GetCustomAttribute<TrackedPropertyAttribute>() != null;
		}

		/// <summary>
		/// Determines whether the property is flagged with the TrackedPropertyAttribute attribute.
		/// </summary>
		/// 
		/// <param name="propertyName">
		/// The property name.
		/// </param>
		/// 
		/// <returns>
		/// True if the property is flagged with the TrackedPropertyAttribute attribute; otherwise, false.
		/// </returns>
		public bool IsTrackedProperty(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return false;
			}

			return _trackedProperties.ContainsKey(propertyName);
		}

		/// <summary>
		/// Determines whether the property is flagged with the TrackedPropertyAttribute attribute.
		/// </summary>
		/// 
		/// <param name="property">
		/// The property reference.
		/// </param>
		/// 
		/// <returns>
		/// True if the property is flagged with the TrackedPropertyAttribute attribute; otherwise, false.
		/// </returns>
		public bool IsTrackedProperty(PropertyInfo property)
		{
			if (property == null)
			{
				return false;
			}

			return this.IsTrackedProperty(property.Name) && _trackedProperties[property.Name] == property;
		}

		#endregion
	}
}

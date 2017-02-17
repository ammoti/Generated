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

	/// <summary>
	/// Provides static methods for reflection purposes.
	/// </summary>
	public static class ReflectionHelper
	{
		/// <summary>
		/// Gets the value of a property.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the returned value.
		/// </typeparam>
		/// 
		/// <param name="targetObject">
		/// Target object.
		/// </param>
		/// 
		/// <param name="propertyName">
		/// Name of the property.
		/// </param>
		/// 
		/// <returns>
		/// The value of the property.
		/// </returns>
		public static T GetPropertyValue<T>(object targetObject, string propertyName)
		{
			T value = default(T);

			if (targetObject != null)
			{
				PropertyInfo property = targetObject.GetType().GetProperty(propertyName);
				if (property != null)
				{
					value = (T)property.GetValue(targetObject, null);
				}
			}

			return value;
		}

		/// <summary>
		/// Sets a value to a property if this property exists on the target object.
		/// </summary>
		/// 
		/// <param name="targetObject">
		/// Target object.
		/// </param>
		/// 
		/// <param name="propertyName">
		/// Name of the property.
		/// </param>
		/// 
		/// <param name="propertyValue">
		/// Value to set.
		/// </param>
		/// 
		/// <returns>
		/// True if the value has been set; otherwise, false.
		/// </returns>
		public static bool SetPropertyValue(object targetObject, string propertyName, object propertyValue)
		{
			bool isSet = false;

			if (targetObject != null)
			{
				PropertyInfo property = targetObject.GetType().GetProperty(propertyName);
				if (property != null)
				{
					property.SetValue(targetObject, propertyValue, null);
					isSet = true;
				}
			}

			return isSet;
		}

		/// <summary>
		/// Maps the target's properties using a Dictionary that contains {Name, Value} properties.
		/// </summary>
		/// 
		/// <param name="target">
		/// Target object.
		/// </param>
		/// 
		/// <param name="properties">
		/// Dictionary that contains {Name, Value} properties.
		/// </param>
		/// 
		/// <param name="throwMappingException">
		/// Value indicating whether exception is thrown when a property cannot be mapped.
		/// </param>
		public static void MapProperties(object target, IDictionary<string, object> properties, bool throwMappingException = true)
		{
			if (properties != null && properties.Count != 0)
			{
				Type targetType = target.GetType();
				PropertyInfo[] props = targetType.GetProperties().Where(p => p.CanWrite).ToArray();

				foreach (var property in properties)
				{
					PropertyInfo pi = props.FirstOrDefault(p => p.Name == property.Key);
					if (pi != null)
					{
						pi.SetValue(target, property.Value);
						continue;
					}

					if (throwMappingException)
					{
						ThrowException.Throw(
							"The '{0}' property (from '{1}.{2}' class) cannot be found or setted",
							property.Key,
							targetType.Namespace,
							targetType.Name);
					}
				}
			}
		}

		public static IList<T> CreatesInstances<T>(params Assembly[] sources)
		{
			IList<T> instances = new List<T>();
			IList<Type> types = new List<Type>();

			Assembly[] assemblies = sources;
			if (assemblies == null || assemblies.Length == 0)
			{
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			}

			foreach (Assembly assembly in assemblies)
			{
				foreach (Type t in assembly.GetTypes().Where(i => i.GetInterface(typeof(T).Name) != null))
				{
					types.Add(t);
				}
			}

			foreach (Type t in types)
			{
				instances.Add((T)Activator.CreateInstance(t));
			}

			return instances;
		}
	}
}

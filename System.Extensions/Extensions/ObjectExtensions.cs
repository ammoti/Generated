// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

// Based on https://raw.githubusercontent.com/Burtsev-Alexey/net-object-deep-copy/master/ObjectExtensions.cs

namespace System
{
	using System.Collections.Generic;
	using System.Reflection;

	public static partial class ObjectExtensions
	{
		#region [ Members ]

		private static readonly MethodInfo _cloneMethod = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Static constructor.
		/// </summary>
		static ObjectExtensions()
		{
			_cloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		#endregion

		#region [ Public Methods ]

		/// <summary>
		/// Creates a deep copy of the current System.Object (even if it is not flagged with Serializable attribute).
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the object to clone.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Object to clone.
		/// </param>
		/// 
		/// <returns>
		/// A deep copy of the current System.Object.
		/// </returns>
		public static T DeepCopy<T>(this T source)
		{
			return (T)DeepCopy((object)source);
		}

		/// <summary>
		/// Creates a deep copy of the current System.Object (even if it is not flagged with Serializable attribute).
		/// </summary>
		/// 
		/// <param name="source">
		/// Object to clone.
		/// </param>
		/// 
		/// <returns>
		/// A deep copy of the current System.Object.
		/// </returns>
		public static object DeepCopy(this object source)
		{
			return InternalDeepCopy(source, new Dictionary<object, object>(new ReferenceEqualityComparer()));
		}

		#endregion

		#region [ Private Methods ]

		private static bool IsPrimitive(this Type type)
		{
			if (type == typeof(string))
			{
				return true;
			}

			return type.IsValueType & type.IsPrimitive;
		}

		private static object InternalDeepCopy(object source, IDictionary<object, object> visited)
		{
			if (source == null)
			{
				return null;
			}

			var typeToReflect = source.GetType();

			if (IsPrimitive(typeToReflect))
			{
				return source;
			}

			if (visited.ContainsKey(source))
			{
				return visited[source];
			}

			if (typeof(Delegate).IsAssignableFrom(typeToReflect))
			{
				return null;
			}

			var cloneobject = _cloneMethod.Invoke(source, null);

			if (typeToReflect.IsArray)
			{
				var arrayType = typeToReflect.GetElementType();
				if (IsPrimitive(arrayType) == false)
				{
					Array clonedArray = (Array)cloneobject;
					clonedArray.ForEach((array, indices) => array.SetValue(InternalDeepCopy(clonedArray.GetValue(indices), visited), indices));
				}
			}

			visited.Add(source, cloneobject);
			CopyFields(source, visited, cloneobject, typeToReflect);
			RecursiveCopyBaseTypePrivateFields(source, visited, cloneobject, typeToReflect);

			return cloneobject;
		}

		private static void RecursiveCopyBaseTypePrivateFields(object source, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
		{
			if (typeToReflect.BaseType != null)
			{
				RecursiveCopyBaseTypePrivateFields(source, visited, cloneObject, typeToReflect.BaseType);
				CopyFields(source, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
			}
		}

		private static void CopyFields(object source, IDictionary<object, object> visited, object cloneobject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
		{
			foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
			{
				if (filter != null && filter(fieldInfo) == false)
				{
					continue;
				}

				if (IsPrimitive(fieldInfo.FieldType))
				{
					continue;
				}

				var originalFieldValue = fieldInfo.GetValue(source);
				var clonedFieldValue = InternalDeepCopy(originalFieldValue, visited);

				fieldInfo.SetValue(cloneobject, clonedFieldValue);
			}
		}

		#endregion
	}
}

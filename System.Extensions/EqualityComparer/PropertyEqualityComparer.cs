// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Reflection;

	public class PropertyEqualityComparer<T> : IEqualityComparer<T>
	{
		#region [ Members ]

		private PropertyInfo _propertyInfo = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Creates a new instance of PropertyEqualityComparer.
		/// </summary>
		/// 
		/// <param name="propertyName">
		/// The name of the property on type T to perform the comparison on.
		/// </param>
		public PropertyEqualityComparer(string propertyName)
		{
			_propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
			if (_propertyInfo == null)
			{
				ThrowException.ThrowArgumentException(
					string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
			}
		}

		#endregion

		#region [IEqualityComparer<T> Implementation ]

		public bool Equals(T x, T y)
		{
			object xValue = _propertyInfo.GetValue(x, null);
			object yValue = _propertyInfo.GetValue(y, null);

			if (xValue == null)
			{
				return yValue == null;
			}

			return xValue.Equals(yValue);
		}

		public int GetHashCode(T obj)
		{
			object propertyValue = _propertyInfo.GetValue(obj, null);

			return (propertyValue != null) ? propertyValue.GetHashCode() : 0;
		}

		#endregion
	}
}

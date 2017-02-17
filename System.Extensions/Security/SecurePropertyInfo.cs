// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Linq.Expressions;

	public class SecurePropertyInfo
	{
		#region [ Constructor ]

		public SecurePropertyInfo(Expression<Func<object>> property, ISecurePropertyProvider provider)
		{
			this.Property = property;
			this.Provider = provider;

			this.PropertyName = ((MemberExpression)property.Body).Member.Name;
		}

		#endregion

		#region [ Properties ]

		public object Property { get; private set; }

		public ISecurePropertyProvider Provider { get; private set; }

		public string PropertyName { get; private set; }

		#endregion
	}
}

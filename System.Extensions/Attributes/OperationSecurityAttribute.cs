// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	/// <summary>
	/// Attribute used to secure method invocation using roles.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	sealed public class OperationSecurityAttribute : Attribute
	{
		#region [ Properties ]

		/// <summary>
		/// Gets or sets the list of the authorized roles that can execute the method.
		/// </summary>
		public List<string> Roles
		{
			get;
			set;
		}

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Private constructor.
		/// </summary>
		private OperationSecurityAttribute()
		{
			this.Roles = new List<string>();
		}

		/// <summary>
		/// Constructor with the list of the roles that can execute the method.
		/// </summary>
		/// 
		/// <param name="roles">
		/// The list of the authorized roles.
		/// </param>
		public OperationSecurityAttribute(params string[] roles)
			: this()
		{
			if (!roles.IsNullOrEmpty())
			{
				this.Roles = new List<string>(roles);
			}
		}

		#endregion
	}
}

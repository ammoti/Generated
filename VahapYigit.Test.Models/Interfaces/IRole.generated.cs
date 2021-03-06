﻿//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was generated by LayerCake Generator v3.7.1.
// http://www.layercake-generator.net
// 
// Changes to this file may cause incorrect behavior AND WILL BE LOST IF 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;

	using VahapYigit.Test.Core;

	[System.CodeDom.Compiler.GeneratedCode("LayerCake Generator", "3.7.1")]
	public partial interface IRole : IEntity
	{
		#region [ Properties ]

		/// <summary>
		/// Gets or sets the CodeRef value (MANDATORY). 
		/// </summary>
		string CodeRef { get; set; }

		/// <summary>
		/// Gets or sets the Name value (MANDATORY). 
		/// </summary>
		string Name { get; set; }


		#endregion

		#region [ References ]

		/// <summary>
		/// Referenced UserRole entity collection by this entity.
		/// </summary>
		TCollection<UserRole> UserRoleCollection { get; set; }

		#endregion
	}
	
	[System.CodeDom.Compiler.GeneratedCode("LayerCake Generator", "3.7.1")]
	public static partial class IRoleExtensions
	{
		public static void MapFrom(this IRole target, IRole source)
		{
			if (source != null)
			{
				if (target != null)
				{
					target.CodeRef = source.CodeRef;
					target.Name = source.Name;
				}
			}
		}

		public static void MapTo(this IRole source, IRole target)
		{
			target.MapFrom(source);
		}
	}
}
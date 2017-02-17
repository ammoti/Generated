// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Class that contains the Crud options.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.CoreOptions", IsReference = true)]
	public class CrudOptions
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public CrudOptions()
		{
			this.WithUserContextSecurity = true;
		}

		/// <summary>
		/// Gets a default CrudOptions instance (WithUserContextSecurity == true).
		/// </summary>
		public static CrudOptions Default
		{
			get { return new CrudOptions(); }
		}

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the WithUserContextSecurity value. If the value is true then Save, Delete and SetLock Crud methods will check the record owner with user context before executing statement.
		/// </summary>
		[DataMember]
		public bool WithUserContextSecurity { get; set; }

		#endregion
	}
}

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
	/// Save options class. Used by Crud Save method.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.Core.SaveOptions", IsReference = true)]
	public class SaveOptions
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public SaveOptions()
		{
			this.CheckUniqueConstraints = true;
			this.SaveChildren = false;
		}

		/// <summary>
		/// Reinitialize the instance with default values (do not save children entities).
		/// </summary>
		public void Clear()
		{
			this.CheckUniqueConstraints = true;
			this.SaveChildren = false;
		}

		/// <summary>
		/// Gets the default instance (check unique constraints and do not save children entities).
		/// </summary>
		public static SaveOptions Default
		{
			get { return new SaveOptions(); }
		}

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the value indicating whether unique constraints are checked. Default value is true.
		/// </summary>
		[DataMember]
		public bool CheckUniqueConstraints { get; set; }

		private bool _saveChildren = false;
		/// <summary>
		/// Gets or sets the value indicating whether the children must be saved or updated on Save call. Default value is false.
		/// </summary>
		[DataMember]
		public bool SaveChildren
		{
			get { return _saveChildren; }
			set { _saveChildren = value; }
		}

		#endregion
	}
}

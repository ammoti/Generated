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

	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	public class UserContextOptions
	{
		/// <summary>
		/// Default constructor (WithContextSecurity = true, RetrieveCurrentLanguageOnly = false).
		/// </summary>
		public UserContextOptions()
		{
			this.WithContextSecurity = true;
			this.RetrieveCurrentLanguageOnly = false;
		}

		/// <summary>
		/// Gets the default UserContextOptions instance (WithContextSecurity = true, RetrieveCurrentLanguageOnly = false).
		/// </summary>
		public static UserContextOptions Default
		{
			get { return new UserContextOptions(); }
		}

		/// <summary>
		/// Gets or sets the value indicating whether the constext security is set.
		/// </summary>
		[DataMember]
		public bool WithContextSecurity { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether only the current language is retrieved (server -&gt; client).
		/// </summary>
		[DataMember]
		public bool RetrieveCurrentLanguageOnly { get; set; }
	}
}

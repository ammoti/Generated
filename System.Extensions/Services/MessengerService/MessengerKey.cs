// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public class MessengerKey
	{
		#region [ Constructors ]

		public MessengerKey()
		{
		}

		public MessengerKey(Type type, string key)
			: this()
		{
			this.Type = type;
			this.Key = key;
		}

		#endregion

		#region [ Properties ]

		public Type Type
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		#endregion
	}
}

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
	using System.Security.Permissions;

	[Serializable]
	public class TechnicalException : Exception, ISerializable
	{
		public TechnicalException()
			: base()
		{
		}

		public TechnicalException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}

		public TechnicalException(Exception innerException, string message, params object[] args)
			: base(string.Format(message, args), innerException)
		{
		}

		protected TechnicalException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#region [ ISerializable Implementation ]

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		#endregion
	}
}

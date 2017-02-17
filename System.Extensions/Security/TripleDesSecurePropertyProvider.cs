// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public class TripleDesSecurePropertyProvider : ISecurePropertyProvider
	{
		#region [ Members ]

		private readonly string _privateKey = null;

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="privateKey">
		/// Enter a value to override the default private key.
		/// </param>
		public TripleDesSecurePropertyProvider(string privateKey)
		{
			_privateKey = privateKey;
		}

		#endregion

		#region [ ISecurePropertyProvider Implementation ]

		public object Secure(object propertyValue)
		{
			if (propertyValue is string)
			{
				if (propertyValue == string.Empty)
				{
					return propertyValue;
				}

				var provider = new TripleDesEncryptorProvider(_privateKey);
				return provider.Encrypt(propertyValue.ToString());
			}

			return propertyValue;
		}

		public object Unsecure(object propertyValue)
		{
			if (propertyValue is string)
			{
				if (propertyValue == string.Empty)
				{
					return propertyValue;
				}

				var provider = new TripleDesEncryptorProvider(_privateKey);
				return provider.Decrypt(propertyValue.ToString());
			}

			return propertyValue;
		}

		#endregion
	}
}

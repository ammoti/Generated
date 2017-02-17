// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary> 
	/// KeyGenerator helper class. 
	/// </summary> 
	public static class KeyGeneratorHelper
	{
		/// <summary> 
		/// Creates a KeyGenerator (implementing IKeyGenerator interface). 
		/// </summary> 
		///  
		/// <typeparam name="T"> 
		/// Key's type. 
		/// </typeparam> 
		///  
		/// <param name="keyGeneratorType"> 
		/// Type of the KeyGenerator to create. 
		/// </param> 
		///  
		/// <returns> 
		/// A KeyGenerator instance. 
		/// </returns> 
		public static IKeyGenerator<T> CreateKeyGenerator<T>(Type keyGeneratorType)
		{
			return (IKeyGenerator<T>)Activator.CreateInstance(keyGeneratorType);
		}

		/// <summary> 
		/// Generates a unique key. 
		/// </summary> 
		///  
		/// <typeparam name="T"> 
		/// Key's type. 
		/// </typeparam> 
		///  
		/// <param name="keyGeneratorType"> 
		/// Type of the KeyGenerator to use. 
		/// </param> 
		///  
		/// <param name="parameters"> 
		/// Parameters to compute the unique key. 
		/// </param> 
		///  
		/// <returns> 
		/// The unique key. 
		/// </returns> 
		public static T GenerateKey<T>(Type keyGeneratorType, object[] parameters = null)
		{
			IKeyGenerator<T> keyGenerator = KeyGeneratorHelper.CreateKeyGenerator<T>(keyGeneratorType);
			return keyGenerator.Generate(parameters);
		}
	}
}

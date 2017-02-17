// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary> 
	/// Interface used to define Key generators. 
	/// </summary> 
	///  
	/// <typeparam name="T"> 
	/// Key's type. 
	/// </typeparam> 
	public interface IKeyGenerator<T>
	{
		/// <summary> 
		/// Generates a unique key. 
		/// </summary> 
		///  
		/// <param name="parameters"> 
		/// Parameters to compute the unique key. 
		/// </param> 
		///  
		/// <returns> 
		/// The unique key. 
		/// </returns> 
		T Generate(object[] parameters);
	}
}

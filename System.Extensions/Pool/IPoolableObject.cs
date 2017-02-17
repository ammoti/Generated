// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public interface IPoolableObject
	{
		void Initialize(object parameter);

		// Resets the state of the poolable object before putting back in the pool.
		void ResetState();
	}
}

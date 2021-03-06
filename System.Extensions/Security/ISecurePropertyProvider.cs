// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public interface ISecurePropertyProvider
	{
		object Secure(object propertyValue);

		object Unsecure(object propertyValue);
	}
}

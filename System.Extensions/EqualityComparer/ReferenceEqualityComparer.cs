// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	public class ReferenceEqualityComparer : EqualityComparer<object>
	{
		#region [ EqualityComparer<object> Implementation ]

		public override bool Equals(object x, object y)
		{
			return ReferenceEquals(x, y);
		}
		public override int GetHashCode(object obj)
		{
			return (obj != null) ? obj.GetHashCode() : 0;
		}

		#endregion
	}
}

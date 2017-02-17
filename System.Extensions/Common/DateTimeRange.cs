// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public class DateTimeRange : Range<DateTime>
	{
		public DateTimeRange(DateTime minimum, DateTime maximum)
			: base(minimum, maximum)
		{
		}

		public DateTimeRange(DateTime? minimum, DateTime? maximum)
			: base(
				(minimum != null) ? minimum.Value : DateTime.MinValue,
				(maximum != null) ? maximum.Value : DateTime.MaxValue)
		{
		}
	}
}

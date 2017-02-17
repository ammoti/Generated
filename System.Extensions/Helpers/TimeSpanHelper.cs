// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public static class TimeSpanHelper
	{
		/// <summary>
		/// Converts a time expression (hh:mm:ss) to a TimeSpan object.
		/// </summary>
		/// 
		/// <param name="expression">
		/// Time expression (hh:mm:ss).
		/// </param>
		/// 
		/// <returns>
		/// The TimeSpan object.
		/// </returns>
		public static TimeSpan ToTimeSpan(string expression)
		{
			if (expression == null)
			{
				ThrowException.ThrowArgumentNullException("expression");
			}

			var blocks = expression.Split(new char[] { ':' });
			string formatError = string.Format("The time expression '{0}' is not correct (should be like 00:00:00)", expression);

			if (blocks.Length != 3)
			{
				ThrowException.ThrowFormatException(formatError);
			}

			bool bParsed;

			int hours;
			bParsed = int.TryParse(blocks[0], out hours); // hours
			if (!bParsed)
			{
				ThrowException.ThrowFormatException(formatError);
			}

			int minutes;
			bParsed = int.TryParse(blocks[1], out minutes); // minutes
			if (!bParsed || minutes < 0 || minutes > 60)
			{
				ThrowException.ThrowFormatException(formatError);
			}

			int seconds;
			bParsed = int.TryParse(blocks[2], out seconds); // seconds
			if (!bParsed || seconds < 0 || seconds > 60)
			{
				ThrowException.ThrowFormatException(formatError);
			}

			return new TimeSpan(hours, minutes, seconds);
		}
	}
}

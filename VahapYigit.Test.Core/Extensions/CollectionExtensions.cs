// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Collections.Generic
{
	using System.Text;

	/// <summary>
	/// Collection extension class.
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Converts a parameter dictionary to a string representation.
		/// </summary>
		/// <param name="parameters">The parameter dictionary.</param>
		/// <returns>The string representation.</returns>
		public static string InString(this IDictionary<string, object> parameters)
		{
			StringBuilder sbText = new StringBuilder(128);

			if (parameters != null)
			{
				IEnumerator<string> iterator = parameters.Keys.GetEnumerator();
				for (int i = 0; i < parameters.Keys.Count; i++)
				{
					iterator.MoveNext();

					if (iterator.Current.ToUpper() == "@PASSWORD")
					{
						sbText.AppendFormat("{0} = '******'", iterator.Current);
					}
					else
					{
						sbText.AppendFormat("{0} = '{1}'", iterator.Current, parameters[iterator.Current]);
					}

					if (i < parameters.Keys.Count - 1)
					{
						sbText.Append(", ");
					}
				}
			}

			return sbText.ToString();
		}
	}
}

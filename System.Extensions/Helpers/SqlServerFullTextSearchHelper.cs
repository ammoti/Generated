// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
//
// -----------------------------------------------

namespace System
{
	public static class SqlServerFullTextSearchHelper
	{
		/// <summary>
		/// Creates the expression for the Full-Text Search (ex, 'red car' -> '"red*" AND "car*"')
		/// </summary>
		/// 
		/// <param name="inputText">
		/// Input text.
		/// </param>
		/// 
		/// <returns>
		/// The expression for the Full-Text Search.
		/// </returns>
		public static string CreateFullTextSearchExpression(string inputText)
		{
			// This code is not really good... but it does what it needs to do :)

			string ftsExpression = string.Empty;

			if (inputText is string)
			{
				string expression = inputText.ToString().ToUpper();

				expression = expression.Replace("'", " ").Replace("\"", string.Empty);
				expression = expression.Replace("-", string.Empty).Replace("* ", " ");

				expression = expression.Replace(" && ", " AND ").Replace(" & ", " AND ");
				expression = expression.Replace(" || ", " OR ").Replace(" | ", " OR ");

				expression = expression.Replace("&", string.Empty);

				string[] terms = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				for (int i = 0; i < terms.Length; i++)
				{
					if (i != 0 && i <= terms.Length - 1 /* bypass first and last iteration */)
					{
						if (!ftsExpression.EndsWith(" AND ") && !ftsExpression.EndsWith(" OR "))
						{
							if (terms[i].ToUpperInvariant() == "AND" || terms[i].ToUpperInvariant() == "OR")
							{
								ftsExpression += string.Format(" {0} ", terms[i].ToUpperInvariant());

								continue;
							}
							else
							{
								ftsExpression += " AND ";
							}
						}
					}

					if (terms[i].StartsWith("(") && terms[i].EndsWith(")"))
					{
						terms[i] = string.Format("\"{0}*\"", terms[i].Substring(1, terms[i].Length - 1));
					}
					else if (terms[i].StartsWith("("))
					{
						terms[i] = string.Format("(\"{0}*\"", terms[i].Substring(1));
					}
					else if (terms[i].EndsWith(")"))
					{
						terms[i] = string.Format("\"{0}*\")", terms[i].Substring(0, terms[i].Length - 1));
					}
					else
					{
						terms[i] = string.Format("\"{0}*\"", terms[i]);
					}

					ftsExpression += terms[i];
				}
			}

			if (ftsExpression.StartsWith("AND "))
			{
				ftsExpression = ftsExpression.Substring(4);
			}

			if (ftsExpression.StartsWith("OR "))
			{
				ftsExpression = ftsExpression.Substring(3);
			}

			if (ftsExpression.EndsWith(" AND "))
			{
				ftsExpression = ftsExpression.Substring(0, ftsExpression.Length - 5);
			}

			if (ftsExpression.EndsWith(" OR "))
			{
				ftsExpression = ftsExpression.Substring(0, ftsExpression.Length - 4);
			}

			int position = ftsExpression.IndexOf("**");
			while (position != -1)
			{
				ftsExpression = ftsExpression.Replace("**", "*");
				position = ftsExpression.IndexOf("**");
			}

			return ftsExpression;
		}
	}
}

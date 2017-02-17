// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	/// <summary>
	/// String extension class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Indicates whether the specified string is null or an System.String.Empty string.
		/// </summary>
		/// 
		/// <param name="@this">
		/// The string to test.
		/// </param>
		/// 
		/// <returns>
		/// True if the value parameter is null or an empty string (""); otherwise, false.
		/// </returns>
		public static bool IsNullOrEmpty(this string @this)
		{
			return string.IsNullOrEmpty(@this);
		}

		/// <summary>
		/// Removes the leading occurrence of the specified trimValue string from the current System.String object.
		/// </summary>
		/// 
		/// <param name="@this">
		/// Input string.
		/// </param>
		/// 
		/// <param name="trimValue">Occurrence to remove.
		/// 
		/// </param>
		/// 
		/// <returns>
		/// The modified string.
		/// </returns>
		public static string TrimStart(this string @this, string trimValue)
		{
			string outString = @this;

			if (!string.IsNullOrEmpty(@this) && !string.IsNullOrEmpty(trimValue) && @this.StartsWith(trimValue))
			{
				outString = @this.Substring(trimValue.Length);
			}

			return outString;
		}

		/// <summary>
		/// Removes the trailing occurrence of the specified trimValue string from the current System.String object.
		/// </summary>
		/// 
		/// <param name="@this">
		/// Input string.
		/// </param>
		/// 
		/// <param name="trimValue">
		/// Occurrence to remove.
		/// </param>
		/// 
		/// <returns>
		/// The modified string.
		/// </returns>
		public static string TrimEnd(this string @this, string trimValue)
		{
			string outString = @this;

			if (!string.IsNullOrEmpty(@this) && !string.IsNullOrEmpty(trimValue) && @this.EndsWith(trimValue))
			{
				outString = @this.Substring(0, @this.Length - trimValue.Length);
			}

			return outString;
		}

		/// <summary>
		/// Performs a safe Substring.
		/// </summary>
		/// <param name="@this">String.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="length">Length.</param>
		/// <returns>The substring.</returns>
		public static string SafeSubString(this string @this, int startIndex, int length)
		{
			return new string((@this ?? string.Empty).Skip(startIndex).Take(length).ToArray());
		}

		/// <summary>
		/// Removes the specified string from the current string.
		/// </summary>
		/// 
		/// <param name="@this">
		/// Input string.</param>
		/// 
		/// <param name="stringToRemoved">
		/// Sub-string to removed.
		/// 
		/// </param>
		/// 
		/// <returns>
		/// The new string.</returns>
		public static string Remove(this string @this, string stringToRemoved)
		{
			string outString = @this;

			if (@this != null && !string.IsNullOrEmpty(stringToRemoved))
			{
				outString = @this.Replace(stringToRemoved, string.Empty);
			}

			return outString;
		}

		// From http://stackoverflow.com/questions/3769457/how-can-i-remove-accents-on-a-string
		public static string RemoveDiacritics(this string @this)
		{
			if (@this == null)
			{
				return null;
			}

			var normalizedString = @this.Normalize(NormalizationForm.FormD);
			var builder = new StringBuilder();

			foreach (var c in normalizedString)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					builder.Append(c);
				}
			}

			return builder.ToString().Normalize(NormalizationForm.FormC);
		}

		/// <summary>
		/// Escapes the newline strings.
		/// </summary>
		/// 
		/// <param name="@this">
		/// Input string.
		/// </param>
		/// 
		/// <returns>
		/// The new string.
		/// </returns>
		public static string Escape(this string @this)
		{
			if (string.IsNullOrEmpty(@this))
			{
				return @this;
			}

			return @this.Replace(Environment.NewLine, "")
						.Replace("\r\n", "")
						.Replace("\n", "");
		}

		/// <summary>
		/// Determines the Guid associated to the string.
		/// </summary>
		/// 
		/// <param name="@this"></param>
		/// Input string.
		/// <returns>
		/// 
		/// The Guid associated to the string
		/// </returns>
		public static Guid ToGuid(this string @this)
		{
			using (var provider = MD5.Create())
			{
				return new Guid(provider.ComputeHash(Encoding.ASCII.GetBytes(@this)));
			}
		}

		/// <summary>
		/// Determines whether the string contains the other one.
		/// </summary>
		/// 
		/// <param name="this">
		/// Input string.
		/// </param>
		/// 
		/// <param name="toCheck">
		/// Other string.
		/// </param>
		/// 
		/// <param name="stringComparison">
		/// StringComparison.
		/// </param>
		/// 
		/// <returns></returns>
		public static bool Contains(this string @this, string toCheck, StringComparison stringComparison)
		{
			return @this.IndexOf(toCheck, stringComparison) >= 0;
		}

		/// <summary>
		/// Determines whether all the words are contained inside the expression.
		/// </summary>
		/// 
		/// <param name="@this">
		/// Input string.
		/// </param>
		/// 
		/// <param name="words">
		/// Array of words.
		/// </param>
		/// 
		/// <param name="ignoreCase">
		/// Value indicating whether the case is ignored.
		/// </param>
		/// 
		/// <returns>
		/// True if all the words are contained inside the expression; otherwise, false.
		/// </returns>
		public static bool ContainsAll(this string @this, string[] words, bool ignoreCase = false)
		{
			if (string.IsNullOrEmpty(@this))
				return false;

			if (words == null || words.Length == 0)
				return true;

			if (ignoreCase)
			{
				@this = @this.ToUpperInvariant();

				for (int i = 0; i < words.Length; i++)
				{
					if (words[i] != null)
						words[i] = words[i].ToUpperInvariant();
				}
			}

			bool bNotFound = false;

			foreach (string word in words)
			{
				if (!@this.Contains(word))
				{
					bNotFound = true;
					break;
				}
			}

			return !bNotFound;
		}
	}

	public class StringIgnoreCaseEqualityComparer : IEqualityComparer<string>
	{
		public bool Equals(string x, string y)
		{
			if (x == null && y == null)
			{
				return true;
			}

			if (x == null || y == null)
			{
				return false;
			}

			return string.Compare(x, y, true) == 0;
		}

		public int GetHashCode(string obj)
		{
			return obj.GetHashCode();
		}
	}
}

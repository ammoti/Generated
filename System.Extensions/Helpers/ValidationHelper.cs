// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ComponentModel.DataAnnotations;
	using System.Text.RegularExpressions;

	/// <summary>
	/// Static class for validations.
	/// </summary>
	public static class ValidationHelper
	{
		#region [ Regex Patterns ]

		/// <summary>
		/// "^[a-z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$"
		/// </summary>
		public const string EmailAddressRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

		#endregion

		#region [ Regexs ]

		private static readonly Regex EmailAddressRegex = new Regex(EmailAddressRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

		#endregion

		/// <summary>
		/// Determines whether the specified value matches the pattern of a valid email address.
		/// </summary>
		/// 
		/// <param name="value">
		/// The value to validate.
		/// </param>
		/// 
		/// <param name="canBeNull">
		/// Indicating whether the value to validate can be null.
		/// </param>
		/// 
		/// <returns>
		/// True if the specified value is valid; otherwise, false.
		/// </returns>
		public static bool IsEmailAddress(string value, bool canBeNull = false)
		{
			if (value == null && canBeNull)
			{
				return true;
			}

			return value != null && EmailAddressRegex.IsMatch(value);
		}

		/// <summary>
		/// Determines whether the specified value matches the pattern of a valid URL format.
		/// </summary>
		/// 
		/// <param name="value">
		/// The value to validate.
		/// </param>
		/// 
		/// <param name="canBeNull">
		/// Indicating whether the value to validate can be null.
		/// </param>
		/// 
		/// <returns>
		/// True if the URL format is valid; otherwise, false.
		/// </returns>
		public static bool IsUrl(string value, bool canBeNull = false)
		{
			return ValidationHelper.IsValid<UrlAttribute>(value, canBeNull);
		}

		/// <summary>
		/// Determines whether the specified value matches the pattern of the specified DataTypeAttribute.
		/// </summary>
		/// 
		/// <typeparam name="TDataTypeAttribute">
		/// Type of the DataTypeAttribute to use for validation.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// The value to validate.
		/// </param>
		/// 
		/// <param name="canBeNull">
		/// Indicating whether the value to validate can be null.
		/// </param>
		/// 
		/// <returns>
		/// True if the value valid; otherwise, false.
		/// </returns>
		public static bool IsValid<TDataTypeAttribute>(string value, bool canBeNull = false) where TDataTypeAttribute : DataTypeAttribute
		{
			if (value == null)
			{
				return canBeNull;
			}

			var validator = Activator.CreateInstance<TDataTypeAttribute>();
			return validator.IsValid(value);
		}
	}
}
// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Used to check conditions.
	/// </summary>
	public static class ConditionChecker
	{
		/// <summary>
		/// Throws a ConditionException exception if the condition is not met.
		/// </summary>
		/// 
		/// <param name="condition">
		/// The condition to meet.
		/// </param>
		/// 
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		public static void Required(bool condition, string message)
		{
			if (!condition)
			{
				ThrowException.ThrowConditionException(message);
			}
		}

		/// <summary>
		/// Throws a ConditionException exception if the condition is not met.
		/// </summary>
		/// 
		/// <param name="condition">
		/// The condition to meet.
		/// </param>
		/// 
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		/// 
		/// <param name="innerException">
		/// The exception that is the cause of the current exception.
		/// </param>
		public static void Required(bool condition, string message, Exception innerException)
		{
			if (!condition)
			{
				ThrowException.ThrowConditionException(message, innerException);
			}
		}

		/// <summary>
		/// Throws a specific exception if the condition is not met.
		/// </summary>
		/// 
		/// <param name="condition">
		/// The condition to meet.
		/// </param>
		/// 
		/// <param name="exception">
		/// The specific exception that is the cause of the current exception.
		/// </param>
		public static void Required(bool condition, Exception exception)
		{
			if (!condition)
			{
				throw exception;
			}
		}

		/// <summary>
		/// Executes a specific action if the condition is not met.
		/// </summary>
		/// 
		/// <param name="condition">
		/// The condition to meet.
		/// </param>
		/// 
		/// <param name="action">
		/// The specific action to execute if the condition is not met.
		/// </param>
		/// 
		/// <param name="parameter">
		/// Optional action parameter.
		/// </param>
		public static void Required(bool condition, Action<object> action, object parameter = null)
		{
			if (!condition)
			{
				action(parameter);
			}
		}
	}
}

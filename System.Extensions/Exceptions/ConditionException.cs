// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// The exception that is thrown when a condition is not met.
	/// </summary>
	[Serializable]
	public class ConditionException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the ConditionException class with a specified error message.
		/// </summary>
		/// 
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		public ConditionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ConditionException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// 
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		/// 
		/// <param name="innerException">
		/// The exception that is the cause of the current exception.
		/// </param>
		public ConditionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// The exception that is thrown by the ServiceLocator.
	/// </summary>
	[Serializable]
	public class ServiceLocatorException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the ServiceLocatorException class with a specified error message.
		/// </summary>
		/// 
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		public ServiceLocatorException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ServiceLocatorException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// 
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		/// 
		/// <param name="innerException">
		/// The exception that is the cause of the current exception.
		/// </param>
		public ServiceLocatorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

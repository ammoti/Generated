// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data.SqlClient;
	using System.IO;
	using System.Reflection;
	using System.Security;

	public static class ThrowException
	{
		#region [ Constructor ]

		static ThrowException()
		{
			_sqlExceptionConstructor = typeof(SqlException).GetConstructor(
				BindingFlags.NonPublic |
				BindingFlags.Instance,
				null,
				new Type[]
				{
					typeof(string),
					typeof(SqlErrorCollection),
					typeof(Exception),
					typeof(Guid)
				},
				null);
		}

		#endregion

		#region [ ArgumentException ]

		public static void ThrowArgumentException(string message)
		{
			throw new ArgumentException(message);
		}

		public static void ThrowArgumentException(string message, Exception innerException)
		{
			throw new ArgumentException(message, innerException);
		}

		public static void ThrowArgumentException(string paramName, string message)
		{
			throw new ArgumentException(message, paramName);
		}

		public static void ThrowArgumentException(string paramName, string message, Exception innerException)
		{
			throw new ArgumentException(message, paramName, innerException);
		}

		#endregion

		#region [ ArgumentNullException ]

		public static void ThrowArgumentNullException(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}

		public static void ThrowArgumentNullException(string message, Exception innerException)
		{
			throw new ArgumentNullException(message, innerException);
		}

		public static void ThrowArgumentNullException(string paramName, string message)
		{
			throw new ArgumentNullException(paramName, message);
		}

		#endregion

		#region [ ArgumentOutOfRangeException ]

		public static void ThrowArgumentOutOfRangeException(string paramName)
		{
			throw new ArgumentOutOfRangeException(paramName);
		}

		#endregion

		#region [ FormatException ]

		public static void ThrowFormatException(string message)
		{
			throw new FormatException(message);
		}

		#endregion

		#region [ InvalidOperationException ]

		public static void ThrowInvalidOperationException(string message)
		{
			throw new InvalidOperationException(message);
		}

		public static void ThrowInvalidOperationException(string message, Exception innerException)
		{
			throw new InvalidOperationException(message, innerException);
		}

		#endregion

		#region [ FileNotFoundException ]

		public static void ThrowFileNotFoundException(string message, string fileName)
		{
			throw new FileNotFoundException(message, fileName);
		}

		#endregion

		#region [ ConfigurationErrorsException ]

		public static void ThrowConfigurationErrorsException(string message)
		{
			throw new ConfigurationErrorsException(message);
		}

		#endregion

		#region [ SecurityException ]

		public static void ThrowSecurityException(string message)
		{
			throw new SecurityException(message);
		}

		#endregion

		#region [ AggregateException ]

		public static void ThrowAggregateException(IEnumerable<Exception> innerExceptions)
		{
			throw new AggregateException(innerExceptions);
		}

		#endregion

		#region [ ConditionException ]

		public static void ThrowConditionException(string message)
		{
			throw new ConditionException(message);
		}

		public static void ThrowConditionException(string message, Exception innerException)
		{
			throw new ConditionException(message, innerException);
		}

		#endregion

		#region [ Exception ]

		public static void Throw(string message, params object[] args)
		{
			string msg = message;

			if (args != null && args.Length != 0)
			{
				msg = string.Format(message, args);
			}

			throw new Exception(msg);
		}

		public static void Throw(Exception innerException, string message, params object[] args)
		{
			string msg = message;

			if (args != null && args.Length != 0)
			{
				msg = string.Format(message, args);
			}

			throw new Exception(msg, innerException);
		}

		#endregion

		#region [ SqlException ]

		// because SqlException has not public constructor...
		private static readonly ConstructorInfo _sqlExceptionConstructor = null;

		public static void ThrowSqlException(string message, SqlException innerException)
		{
			throw CreateSqlException(message, innerException);
		}

		private static SqlException CreateSqlException(string message, SqlException innerException)
		{
			return (SqlException)_sqlExceptionConstructor.Invoke(
				new object[]
				{
					message,
					innerException.Errors,
					innerException,
					innerException.ClientConnectionId
				});
		}

		#endregion
	}
}

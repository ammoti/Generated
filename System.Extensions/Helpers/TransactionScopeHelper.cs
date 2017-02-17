// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Transactions;

	/// <summary>
	/// TransactionScope extension class.
	/// </summary>
	public static class TransactionScopeHelper
	{
		/// <summary>
		/// Creates a default transactionScope object (TransactionScopeOption.Required / IsolationLevel.ReadCommitted / TransactionManager.MaximumTimeout).
		/// </summary>
		/// 
		/// <param name="timeout">
		/// The transactionScope timeout value in seconds. If not defained the TransactionManager.MaximumTimeout is set.
		/// </param>
		/// 
		/// <returns>
		/// TransactionScope object.
		/// </returns>
		public static TransactionScope CreateDefaultTransactionScope(TimeSpan? timeout = null)
		{
			if (timeout == null)
			{
				timeout = TransactionManager.MaximumTimeout;
			}

			return new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions()
				{
					IsolationLevel = IsolationLevel.ReadCommitted,
					Timeout = timeout.Value
				}
			);
		}
	}
}
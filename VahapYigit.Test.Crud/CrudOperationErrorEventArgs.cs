// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Crud
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// EventArgs for Crud calls on error.
	/// </summary>
	public class CrudOperationErrorEventArgs : EventArgs
	{
		/// <summary>
		/// Exception object.
		/// </summary>
		public Exception Exception = null;

		/// <summary>
		/// Name of the stored procedure that failed.
		/// </summary>
		public string Procedure = null;

		/// <summary>
		/// Input parameters of the stored procedure.
		/// </summary>
		public IDictionary<string, object> Parameters = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="exception">Exception object.</param>
		/// <param name="procedure">Name of the stored procedure that failed.</param>
		/// <param name="parameters">Input parameters of the stored procedure.</param>
		public CrudOperationErrorEventArgs(Exception exception, string procedure, IDictionary<string, object> parameters)
		{
			this.Exception = exception;
			this.Procedure = procedure;
			this.Parameters = parameters;
		}
	}
}
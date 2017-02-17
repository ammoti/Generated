// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// ILoggerService interface.
	/// </summary>
	public interface ILoggerService
	{
		/// <summary>
		/// Gets or sets the source of the LOG (file path, eventlog's name, etc).
		/// </summary>
		string LogSource { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the logger is enabled.
		/// </summary>
		bool IsEnabled { get; set; }

		/// <summary>
		/// Write a message in the LOG.
		/// </summary>
		/// 
		/// <param name="instance">
		/// Instance to the caller (enter the keyword 'this').
		/// </param>
		/// 
		/// <param name="logStatus">
		/// Log entry status.
		/// </param>
		/// 
		/// <param name="message">
		/// Message to write.
		/// </param>
		/// 
		/// <param name="args">
		/// Optional parameters to format the message to write.
		/// </param>
		void WriteLine(object instance, LogStatusEnum logStatus, string message, params object[] args);

		/// <summary>
		/// Trace an exception.
		/// </summary>
		/// 
		/// <param name="instance">
		/// Instance to the caller (enter the keyword 'this').
		/// </param>
		/// 
		/// <param name="exception">
		/// Execption object.
		/// </param>
		/// 
		/// <param name="description">
		/// Optional description.
		/// </param>
		void TraceException(object instance, Exception exception, string description = null);
	}
}

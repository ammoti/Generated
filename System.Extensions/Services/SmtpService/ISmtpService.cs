// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Net.Mail;
	using System.Threading.Tasks;

	/// <summary>
	/// SmtpService interface.
	/// </summary>
	public interface ISmtpService
	{
		/// <summary>
		/// Gets a new pre-configured SmtpClient instance using the smtpServiceCongiguration section.
		/// </summary>
		SmtpClient GetSmtpClientInstance();

		/// <summary>
		/// Sends a mail.
		/// The 'from' value comes from the configuration file and can be overrided from options.
		/// </summary>
		/// 
		/// <param name="to">
		/// To field.
		/// </param>
		/// 
		/// <param name="subject">
		/// Mail subject.
		/// </param>
		/// 
		/// <param name="message">
		/// Mail message.
		/// </param>
		/// 
		/// <param name="options">
		/// Options (optional).
		/// </param>
		void Send(string to, string subject, string message, SmtpServiceOptions options = null);

		/// <summary>
		/// Sends an asynchronous mail.
		/// The 'from' value comes from the configuration file and can be overrided from options.
		/// </summary>
		/// 
		/// <param name="to">
		/// To field.
		/// </param>
		/// 
		/// <param name="subject">
		/// Mail subject.
		/// </param>
		/// 
		/// <param name="message">
		/// Mail message.
		/// </param>
		/// 
		/// <param name="options">
		/// Options (optional).
		/// </param>
		Task SendAsync(string to, string subject, string message, SmtpServiceOptions options = null);
	}
}

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
	/// Quick access to the ISmtpService defined in the configuration file (ServiceLocator definition).
	/// </summary>
	public class SmtpServiceHelper : Singleton<SmtpServiceHelper>, ISmtpService
	{
		#region [ Members ]

		private static readonly ISmtpService _smtpService = ServiceLocator.Current.Resolve<ISmtpService>();

		#endregion

		#region [ ISmtpService Implementation ]

		/// <summary>
		/// Gets a new pre-configured SmtpClient instance using the smtpServiceCongiguration section.
		/// </summary>
		public SmtpClient GetSmtpClientInstance()
		{
			return _smtpService.GetSmtpClientInstance();
		}

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
		public void Send(string to, string subject, string message, SmtpServiceOptions options = null)
		{
			_smtpService.Send(to, subject, message, options);
		}

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
		public async Task SendAsync(string to, string subject, string message, SmtpServiceOptions options = null)
		{
			await _smtpService.SendAsync(to, subject, message, options);
		}

		#endregion
	}
}

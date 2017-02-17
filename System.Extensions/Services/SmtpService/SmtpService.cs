// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Mail;
	using System.Threading.Tasks;

	/// <summary>
	/// SmtpService class.
	/// </summary>
	public class SmtpService : ISmtpService
	{
		#region [ Members ]

		private static readonly SmtpServiceSection _config = SmtpServiceSectionHandler.GetSection();

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Default constructor.
		/// Uses &lt;smtpServiceConfiguration&gt; section from application configuration file.
		/// </summary>
		public SmtpService()
		{
			if (_config == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the smtpServiceConfiguration section from the configuration file");
			}
		}

		#endregion

		#region [ ISmtpService Implementation ]

		/// <summary>
		/// Gets a new pre-configured SmtpClient instance using the smtpServiceCongiguration section.
		/// </summary>
		public SmtpClient GetSmtpClientInstance()
		{
			return new SmtpClient
			{
				Host = _config.Host,
				Port = _config.Port,

				DeliveryMethod = SmtpDeliveryMethod.Network,
				EnableSsl = _config.EnableSsl,
				UseDefaultCredentials = _config.UseDefaultCredentials,

				Credentials = (!_config.UseDefaultCredentials) ? new NetworkCredential(_config.Username, _config.Password) : CredentialCache.DefaultNetworkCredentials
			};
		}

		/// <summary>
		/// Sends a mail.
		/// </summary>
		/// 
		/// <param name="to">
		/// To field (destinator).
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
		/// SmtpService options (optional).
		/// </param>
		public void Send(string to, string subject, string message, SmtpServiceOptions options = null)
		{
			try
			{
				using (var smtpClient = this.GetSmtpClientInstance())
				using (var mailMessage = CreateMailMessage(to, subject, message, options))
				{
					smtpClient.Send(mailMessage);
				}
			}
			catch (SmtpException x)
			{
				LoggerServiceHelper.Current.TraceException(this, x);

				throw;
			}
		}

		/// <summary>
		/// Sends an asynchronous mail.
		/// </summary>
		/// 
		/// <param name="to">
		/// To field (destinator).
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
		/// SmtpService options (optional).
		/// </param>
		public async Task SendAsync(string to, string subject, string message, SmtpServiceOptions options = null)
		{
			try
			{
				using (var smtpClient = this.GetSmtpClientInstance())
				using (var mailMessage = CreateMailMessage(to, subject, message, options))
				{
					await smtpClient.SendMailAsync(mailMessage);
				}
			}
			catch (Exception x)
			{
				LoggerServiceHelper.Current.TraceException(this, x);

				throw;
			}
		}

		#endregion

		#region [ Private Methods ]

		private static MailMessage CreateMailMessage(string to, string subject, string message, SmtpServiceOptions options = null)
		{
			if (options == null)
			{
				options = new SmtpServiceOptions();
			}

			if (string.IsNullOrEmpty(options.From))
			{
				options.From = _config.From;
			}

			if (_config.Redirection != null)
			{
				to = _config.Redirection;

				options.Cc = null;
				options.Bcc = null;

				subject = string.Format("[REDIRECTION] {0}", subject);
			}
			else
			{
				if (!string.IsNullOrEmpty(_config.Cc))
				{
					options.Cc = string.Concat(options.Cc, ",", _config.Cc);
				}

				if (!string.IsNullOrEmpty(_config.Bcc))
				{
					options.Bcc = string.Concat(options.Bcc, ",", _config.Bcc);
				}
			}

			var mailMessage = new MailMessage();

			foreach (var toEntry in ExtractEmails(to))
			{
				mailMessage.To.Add(new MailAddress(toEntry));
			}

			foreach (var ccEntry in ExtractEmails(options.Cc))
			{
				mailMessage.CC.Add(new MailAddress(ccEntry));
			}

			foreach (var bccEntry in ExtractEmails(options.Bcc))
			{
				mailMessage.Bcc.Add(new MailAddress(bccEntry));
			}

			string senderName = (!string.IsNullOrEmpty(options.SenderName)) ? options.SenderName : _config.SenderName;

			mailMessage.From = new MailAddress(options.From, senderName);

			mailMessage.Subject = subject;
			mailMessage.Body = message;

			mailMessage.IsBodyHtml = options.IsHtmlBody;

			if (options.Files != null)
			{
				foreach (var file in options.Files)
				{
					mailMessage.Attachments.Add(new Attachment(file, FileHelper.GetMimeType(file)));
				}
			}

			return mailMessage;
		}

		/// <summary>
		/// Extracts emails from inline string.
		/// </summary>
		/// 
		/// <param name="emailsInLine">
		/// Inline string containing emails separated with comma, semicolon or space.
		/// </param>
		/// 
		/// <returns>
		/// A list that contains emails.
		/// </returns>
		private static IEnumerable<string> ExtractEmails(string emailsInLine)
		{
			var emails = new List<string>();

			if (emailsInLine != null)
			{
				var explode = new Func<string, IEnumerable<string>>((input) =>
				{
					return input.Split(new char[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				});

				emails = explode(emailsInLine).Select(e => e.Trim()).ToList();
			}

			return emails;
		}

		#endregion
	}
}

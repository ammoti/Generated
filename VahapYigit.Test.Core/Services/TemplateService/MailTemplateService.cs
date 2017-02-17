// -----------------------------------------------
// This file is part of the VahapYigit.Test.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;
	using System.Xml;

	public class MailTemplateService : ITemplateService
	{
		#region [ Members ]

		private readonly string _templatePath = null;

		private readonly MailTemplate _mailTemplate = null;
		private readonly ISmtpService _smtpService = null;

		#endregion

		#region [ Constructors ]

		public MailTemplateService(string templateName)
			: this(templateName, SmtpServiceHelper.Current)
		{
		}

		public MailTemplateService(string templateName, ISmtpService smtpService)
		{
			if (smtpService == null)
			{
				ThrowException.ThrowArgumentNullException("smtpService");
			}

			var config = MailTemplateServiceSectionHandler.GetSection();

			_templatePath = string.Format("{0}.xml", Path.Combine(config.Path, templateName));
			_mailTemplate = new MailTemplate();

			_smtpService = smtpService;
		}

		#endregion

		#region [ ITemplateService Implementation ]

		public void Load(string culture, IDictionary<string, object> tokens)
		{
			if (string.IsNullOrEmpty(culture))
			{
				ThrowException.ThrowArgumentNullException("culture");
			}

			if (!File.Exists(_templatePath))
			{
				ThrowException.ThrowFileNotFoundException(
					string.Format("Cannot find the {0} template!", _templatePath),
					_templatePath);
			}

			culture = culture.Remove("-");

			XmlDocument xmlTemplate = new XmlDocument();
			xmlTemplate.Load(_templatePath);

			XmlNode rootNode = xmlTemplate.DocumentElement;

			XmlNode templateNode = rootNode.SelectSingleNode(
				string.Format("Template[ @culture = \"{0}\" ]", culture));

			if (templateNode == null)
			{
				templateNode = rootNode.SelectSingleNode(
				   string.Format("Template[ @culture = \"{0}\" ]", Cultures.Default));
			}

			if (templateNode == null)
			{
				ThrowException.Throw(
					"Error while loading the {0} template: cannot found neither {1} nor {2} culture!",
					Path.GetFileName(_templatePath), culture, Cultures.Default);
			}

			_mailTemplate.Subject = templateNode["Subject"].InnerText;
			_mailTemplate.Message = templateNode["Message"].InnerText;

			if (tokens != null)
			{
				foreach (string key in tokens.Keys)
				{
					_mailTemplate.Subject = ReplaceToken(_mailTemplate.Subject, key, tokens[key]);
					_mailTemplate.Message = ReplaceToken(_mailTemplate.Message, key, tokens[key]);
				}
			}
		}

		#endregion

		#region [ Public Methods ]

		public void SendByMail(string email, SmtpServiceOptions options = null)
		{
			_smtpService.Send(email, _mailTemplate.Subject, _mailTemplate.Message, options);
		}

		public async Task SendByMailAsync(string email, SmtpServiceOptions options = null)
		{
			await _smtpService.SendAsync(email, _mailTemplate.Subject, _mailTemplate.Message, options);
		}

		#endregion

		#region [ Private Methods ]

		private static string ReplaceToken(string text, string key, object value)
		{
			if (text == null) return null;
			return text.Replace(key, (value == null) ? string.Empty : value.ToString());
		}

		#endregion
	}
}

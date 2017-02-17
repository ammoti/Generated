// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Xml;

	/// <summary>
	/// SmtpServiceSection class.
	/// </summary>
	public class SmtpServiceSection
	{
		private string _hostname = "127.0.0.1";
		private int _port = 25;

		private string _username = null;
		private string _password = null;

		private string _senderName = null;
		private string _from = null;
		private string _cc = null;
		private string _bcc = null;
		private string _redirection = null;

		private bool _enableSsl = false;
		private bool _useDefaultCredentials = true;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SmtpServiceSection()
		{
		}

		/// <summary>
		/// Loads the smtpConfiguration Xml section.
		/// </summary>
		/// <param name="sectionNode">The smtpConfiguration root node.</param>
		internal void Load(XmlNode sectionNode)
		{
			IList<string> attributeNames = new List<string>()
            {
                "hostname", "port", "username", "password", "senderName", "from", "cc", "bcc", "redirection", "enableSsl", "useDefaultCredentials"
            };

			foreach (string attributeName in attributeNames)
			{
				if (sectionNode.Attributes[attributeName] == null)
				{
					ThrowException.ThrowConfigurationErrorsException(
						string.Format("Cannot find the '{0}' attribute on 'smtpServiceConfiguration'", attributeName));
				}

				if (attributeName == "hostname")
				{
					_hostname = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "port")
				{
					_port = Convert.ToInt32(sectionNode.Attributes[attributeName].InnerText);
				}
				else if (attributeName == "username")
				{
					_username = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "password")
				{
					_password = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "senderName")
				{
					_senderName = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "from")
				{
					_from = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "cc")
				{
					_cc = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "bcc")
				{
					_bcc = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "redirection")
				{
					_redirection = sectionNode.Attributes[attributeName].InnerText;
				}
				else if (attributeName == "enableSsl")
				{
					_enableSsl = Convert.ToBoolean(sectionNode.Attributes[attributeName].InnerText);
				}
				else if (attributeName == "useDefaultCredentials")
				{
					_useDefaultCredentials = Convert.ToBoolean(sectionNode.Attributes[attributeName].InnerText);
				}
			}

			if (_username.Length == 0 || _password.Length == 0)
			{
				_username = null;
				_password = null;
			}

			if (_redirection.Length == 0)
			{
				_redirection = null;
			}
		}

		#region [ Properties ]

		/// <summary>
		/// Gets the Smtp server hostname.
		/// </summary>
		public string Host
		{
			get { return _hostname; }
		}

		/// <summary>
		/// Gets the Smtp server port.
		/// </summary>
		public int Port
		{
			get { return _port; }
		}

		/// <summary>
		/// Gets the Smtp server username (for credentials).
		/// </summary>
		public string Username
		{
			get { return _username; }
		}

		/// <summary>
		/// Gets the Smtp server password (for credentials).
		/// </summary>
		public string Password
		{
			get { return _password; }
		}

		/// <summary>
		/// Gets the sender name to set to the mail.
		/// </summary>
		public string SenderName
		{
			get { return _senderName; }
		}

		/// <summary>
		/// Gets the expeditor's email.
		/// </summary>
		public string From
		{
			get { return _from; }
		}

		/// <summary>
		/// Gets the Cc value.
		/// </summary>
		public string Cc
		{
			get { return _cc; }
		}

		/// <summary>
		/// Gets the Bcc value.
		/// </summary>
		public string Bcc
		{
			get { return _bcc; }
		}

		/// <summary>
		/// Gets the email redirection (when a mail is sent, To field is overrided by this value if defined).
		/// </summary>
		public string Redirection
		{
			get { return _redirection; }
		}

		/// <summary>
		/// Gets or sets the value indicating whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection.
		/// </summary>
		public bool EnableSsl
		{
			get { return _enableSsl; }
		}

		/// <summary>
		/// Gets or sets the value indicating that controls whether the DefaultCredentials are sent with requests.
		/// </summary>
		public bool UseDefaultCredentials
		{
			get { return _useDefaultCredentials; }
		}

		#endregion
	}
}

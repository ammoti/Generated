// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Security;
	using System.Security;
	using System.Security.Cryptography.X509Certificates;
	using System.Security.Policy;

	public static class ServerCertificateValidationHelper
	{
		#region [ Members ]

		private static readonly IList<string> _trustedCertificateDistinguishedNames = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Static constructor.
		/// </summary>
		static ServerCertificateValidationHelper()
		{
			_trustedCertificateDistinguishedNames = new List<string>();
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Adds the distinguished name of certificates to trust.
		/// </summary>
		/// 
		/// <param name="distinguishedName">
		/// Distinguished name of the certificate to trust.
		/// </param>
		public static void AddTrustedCertificateDistinguishedName(string distinguishedName)
		{
			if (!string.IsNullOrEmpty(distinguishedName))
			{
				distinguishedName = distinguishedName.ToLowerInvariant();

				if (!_trustedCertificateDistinguishedNames.Contains(distinguishedName))
				{
					_trustedCertificateDistinguishedNames.Add(distinguishedName);
				}
			}
		}

		#endregion

		#region [ Validate Methods ]

		public static bool ValidateRemoteCertificateCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		public static bool ValidateTrustedZoneRemoteCertificateCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			// Method based on http://msdn.microsoft.com/en-us/library/office/bb408523(v=exchg.140).aspx

			if (sslPolicyErrors == SslPolicyErrors.None /* valid certificate */)
			{
				return true;
			}

			HttpWebRequest request = sender as HttpWebRequest;
			if (request.RequestUri.IsLoopback) // Accept all local certificates...
			{
				return true;
			}

			IList<SecurityZone> trustedZones = new List<SecurityZone>
            {
                SecurityZone.MyComputer, SecurityZone.Intranet, SecurityZone.Trusted
            };

			Zone zone = Zone.CreateFromUrl(((HttpWebRequest)sender).RequestUri.ToString());
			if (trustedZones.Contains(zone.SecurityZone))
			{
				return true;
			}

			foreach (string name in _trustedCertificateDistinguishedNames)
			{
				if (certificate.Subject.ToLowerInvariant().Contains(name))
				{
					return true;
				}
			}

			// If there are errors in the certificate chain look at each error to determine the cause...
			if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
			{
				if (chain != null && chain.ChainStatus != null)
				{
					foreach (X509ChainStatus status in chain.ChainStatus)
					{
						if (certificate.Subject == certificate.Issuer && status.Status == X509ChainStatusFlags.UntrustedRoot)
						{
							continue; // Self-signed certificates with an untrusted root are valid.
						}

						if (status.Status != X509ChainStatusFlags.NoError)
						{
							// If there are any other errors in the certificate chain,
							// the certificate is invalid, so the method returns false.

							return false;
						}
					}
				}

				// When processing reaches this line, the only errors in the certificate chain are untrusted root errors for self-signed certificates.
				// These certificates are valid for default Exchange server installations, so return true.

				return true;
			}

			return false;
		}

		#endregion
	}
}

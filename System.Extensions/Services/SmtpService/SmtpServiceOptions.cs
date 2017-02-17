// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	/// <summary>
	/// Class that contains the SmtpService options.
	/// </summary>
	public class SmtpServiceOptions
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public SmtpServiceOptions()
		{
			this.IsHtmlBody = false;
			this.Files = new List<string>();
		}

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the From field.
		/// </summary>
		public string From { get; set; }

		/// <summary>
		/// Gets or sets the Cc field.
		/// </summary>
		public string Cc { get; set; }

		/// <summary>
		/// Gets or sets the Bcc field.
		/// </summary>
		public string Bcc { get; set; }

		/// <summary>
		/// Gets or sets the attachments.
		/// </summary>
		public IList<string> Files { get; set; }

		/// <summary>
		/// Gets or sets the value indicating the sender name.
		/// </summary>
		public string SenderName { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the body is Html format. Default value is false (Text).
		/// </summary>
		public bool IsHtmlBody { get; set; }

		#endregion
	}
}

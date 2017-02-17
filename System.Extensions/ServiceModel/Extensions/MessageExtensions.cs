// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.ServiceModel.Channels // System.ServiceModel.Web.dll
{
	public static class MessageExtensions
	{
		/// <summary>
		/// Gets the format used for the message body.
		/// </summary>
		///
		/// <param name="message">
		/// The message.
		/// </param>
		///
		/// <returns>
		/// The System.ServiceModel.Channels.WebContentFormat that specifies the format used for the message body.
		/// </returns>
		public static WebContentFormat GetContentFormat(this Message message)
		{
			WebContentFormat format = WebContentFormat.Default;

			if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
			{
				format = ((WebBodyFormatMessageProperty)message.Properties[WebBodyFormatMessageProperty.Name]).Format;
			}

			return format;
		}
	}
}

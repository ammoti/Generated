// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;

	public class ProcessMessageEventArgs : EventArgs
	{
		public ProcessMessageEventArgs(ProcessMessageType type, string message, params object[] args)
		{
			this.Type = type;

			this.Time = DateTime.Now.ToString("HH:mm:ss");
			this.Message = string.Format(message, args);
		}

		public ProcessMessageType Type
		{
			get;
			set;
		}

		public string Time
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}
	}
}

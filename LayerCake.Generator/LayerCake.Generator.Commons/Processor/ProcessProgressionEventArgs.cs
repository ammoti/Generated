// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;

	public class ProcessProgressionEventArgs : EventArgs
	{
		public ProcessProgressionEventArgs(double percentage)
		{
			this.Percentage = percentage;
		}

		public double Percentage { get; set; }
	}
}

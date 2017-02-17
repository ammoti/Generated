// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;

	public interface INotifyActiveChanged
	{
		event EventHandler IsActiveChanged;

		bool IsActive { get; set; }
	}
}

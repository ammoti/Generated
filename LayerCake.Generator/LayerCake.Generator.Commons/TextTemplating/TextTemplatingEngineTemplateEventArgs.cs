// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;

	public class TextTemplatingEngineTemplateEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="template">
		/// Template.
		/// </param>
		public TextTemplatingEngineTemplateEventArgs(string template)
		{
			this.Template = template;
		}

		/// <summary>
		/// Template.
		/// </summary>
		public string Template
		{
			get;
			set;
		}
	}
}

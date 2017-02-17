// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;
	using System.Collections.Generic;

	[Serializable]
	public class TextTemplatingProcessContext
	{
		public ProcessorContext ProcessorContext { get; set; }

		public SmoContext SmoContext { get; set; }

		public ModelDescriptorSchema Schema { get; set; }

		public ModelDescriptorSchemaTable Table { get; set; }

		public IList<BusinessClassInfo> BusinessClasses { get; set; }

		public BusinessClassInfo BusinessClass { get; set; }
	}
}

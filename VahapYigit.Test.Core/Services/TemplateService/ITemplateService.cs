// -----------------------------------------------
// This file is part of the VahapYigit.Test.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Collections.Generic;

	public interface ITemplateService
	{
		void Load(string culture, IDictionary<string, object> tokens);
	}
}

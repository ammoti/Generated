// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using JsonConfig;

	using System.IO;

	public static class JsonConfigHelper
	{
		public static ConfigObject LoadJsonFile(string jsonFile)
		{
			return Config.ApplyJson(File.ReadAllText(jsonFile));
		}
	}
}

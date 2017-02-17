// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Collections.Generic;
	using System.Linq;

	public static partial class TranslationHelper
	{
		public static bool HasTranslation(this IEnumerable<TranslationEnum> translations, TranslationEnum translation)
		{
			return translations.Contains(translation);
		}
	}
}

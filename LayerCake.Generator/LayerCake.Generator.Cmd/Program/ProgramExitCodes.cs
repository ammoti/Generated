// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Cmd
{
	class ProgramExitCodes
	{
		public static readonly int Ok = 0;

		public static readonly int ConfigFileNotFound = -1;

		public static readonly int InvalidArguments = -2;

		public static readonly int ProcessException = -3;
	}
}

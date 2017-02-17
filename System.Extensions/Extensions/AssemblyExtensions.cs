// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Reflection
{
	using System.Diagnostics;

	public static class AssemblyExtensions
	{
		/// <summary>
		/// Determines whether the assembly has been compiled in Debug or Release mode.
		/// </summary>
		/// 
		/// <param name="assembly">
		/// The assembly to test.
		/// </param>
		/// 
		/// <returns>
		/// True if the assembly was built in debug; otherwise, false.
		/// </returns>
		public static bool IsDebugAssembly(this Assembly assembly)
		{
			foreach (object attr in assembly.GetCustomAttributes(false))
			{
				if (attr is DebuggableAttribute)
				{
					return (attr as DebuggableAttribute).IsJITTrackingEnabled;
				}
			}

			return false;
		}
	}
}

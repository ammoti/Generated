// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	/// <summary>
	/// Contains property assemblies shared values.
	/// </summary>
	public static class GlobalAssemblyInfo
	{
		/// <summary>
		/// Company name.
		/// </summary>
		public const string AssemblyCompany = "LayerCake-Generator.Net";

		/// <summary>
		/// Company copyright.
		/// </summary>
		public const string AssemblyCopyright = "Copyright © LayerCake-Generator.Net 2012, 2015";

		/// <summary>
		/// Product trademark.
		/// </summary>
		public const string AssemblyTrademark = "LayerCake-Generator.Net";

		/// <summary>
		/// Assembly culture.
		/// </summary>
		public const string AssemblyCulture = "";

#if RELEASE
		/// <summary>
		/// Assembly compilation mode (RELEASE).
		/// </summary>
		public const string AssemblyConfiguration = "RELEASE";
#else
		/// <summary>
		/// Assembly compilation mode (DEBUG).
		/// </summary>
		public const string AssemblyConfiguration = "DEBUG";
#endif

		/// <summary>
		/// Assembly version.
		/// </summary>
		public const string AssemblyVersion = "3.7.1.0";

		/// <summary>
		/// Assembly file version.
		/// </summary>
		public const string AssemblyFileVersion = "3.7.1.0";
	}
}

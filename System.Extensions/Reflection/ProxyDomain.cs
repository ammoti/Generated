// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Reflection
{
	using System.IO;

	/// <summary>
	/// Class used to load assemblies.
	/// </summary>
	public class ProxyDomain : MarshalByRefObject
	{
		/// <summary>
		/// Loads an assembly with or without the PDB symbols.
		/// </summary>
		/// 
		/// <param name="assemblyPath">
		/// Assembly path.
		/// </param>
		/// 
		/// <param name="withPdbSymbols">
		/// Value indicating whether the PDB symbols are loaded.
		/// </param>
		/// 
		/// <returns>
		/// The Assembly object.
		/// </returns>
		public Assembly LoadAssembly(string assemblyPath, bool withPdbSymbols = true)
		{
			Assembly assembly = null;

			try
			{
				assembly = (withPdbSymbols) ?
					this.LoadAssemblyWithPdbSymbols(assemblyPath) :
					this.LoadAssembly(assemblyPath);

			}
			catch (Exception x)
			{
				ThrowException.ThrowInvalidOperationException(x.Message, x);
			}

			return assembly;
		}

		/// <summary>
		/// Loads an assembly with the PDB symbols.
		/// </summary>
		/// 
		/// <param name="assemblyPath">
		/// Assembly path.
		/// </param>
		/// 
		/// <returns>
		/// The Assembly object.
		/// </returns>
		private Assembly LoadAssemblyWithPdbSymbols(string assemblyPath)
		{
			string pdbPath = Path.GetDirectoryName(assemblyPath);

			pdbPath = Path.Combine(pdbPath, Path.GetFileNameWithoutExtension(assemblyPath));
			pdbPath = string.Format("{0}.pdb", pdbPath);

			byte[] assData = File.ReadAllBytes(assemblyPath);
			byte[] pdbData = File.ReadAllBytes(pdbPath);

			return Assembly.Load(assData, pdbData);
		}

		/// <summary>
		/// Loads an assembly.
		/// </summary>
		/// 
		/// <param name="assemblyPath">
		/// Assembly path.
		/// </param>
		/// 
		/// <returns>
		/// The Assembly object.
		/// </returns>
		private Assembly LoadAssembly(string assemblyPath)
		{
			return Assembly.LoadFrom(assemblyPath);
		}
	}
}

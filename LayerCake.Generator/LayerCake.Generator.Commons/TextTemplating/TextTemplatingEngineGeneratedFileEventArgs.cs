// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;

	public class TextTemplatingEngineFileGeneratedEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="file">
		/// Full path of the file.
		/// </param>
		/// 
		/// <param name="isAdded">
		/// Value indicating whteher the file is attached to the project (csproj, sqlproj, etc).
		/// </param>
		public TextTemplatingEngineFileGeneratedEventArgs(string file, bool isAttached)
		{
			this.File = file;
			this.IsAttached = isAttached;
		}

		public string File
		{
			get;
			set;
		}

		public bool IsAttached
		{
			get;
			set;
		}
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	public class TextTemplatingParameters
	{
		#region [ Constructors ]

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="templateFilePath">
		/// Path of the T4 Template to process.
		/// </param>
		/// 
		/// <param name="outputFilePath">
		/// Path of the T4 Template to process.
		/// </param>
		/// 
		/// <param name="overrideIfExists">
		/// Value indicating whether the output file must override an existing one.
		/// </param>
		/// 
		/// <param name="addToProject">
		/// Value indicating whether the output file has to be attached to the C# project.
		/// </param>
		public TextTemplatingParameters(string templateFilePath, string outputFilePath, bool overrideIfExists, bool addToProject)
		{
			this.TemplateFilePath = templateFilePath;
			this.OutputFilePath = outputFilePath;

			this.OverrideIfExists = overrideIfExists;
			this.AddToProject = addToProject;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the path of the T4 Template to process.
		/// </summary>
		public string TemplateFilePath
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the path of the output file.
		/// </summary>
		public string OutputFilePath
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value indicating whether the output file must override an existing one.
		/// </summary>
		public bool OverrideIfExists
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value indicating whether the output file has to be attached to the C# project.
		/// </summary>
		public bool AddToProject
		{
			get;
			set;
		}

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	public class TextTemplatingOutputFileDefinition
	{
		#region [ Constants ]

		// When processing the TextTemplatingEngine generates temp files using the TmpOutputFilePattern pattern.
		// Once the whole process is finished, if there is no error, all temp files override originals (if required).

		private static readonly string TmpOutputFilePattern = "{0}.lcg-tmp";

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="path">
		/// Path of the output file.
		/// </param>
		/// 
		/// <param name="addToProject">
		/// Value indicating whether the output file has to be attached to the C# project.
		/// </param>
		public TextTemplatingOutputFileDefinition(string path, bool addToProject)
		{
			this.Path = path;
			this.AddToProject = addToProject;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the path of the output file.
		/// </summary>
		public string Path
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the path of the temp output file.
		/// </summary>
		public string TempPath
		{
			get
			{
				if (!string.IsNullOrEmpty(this.Path))
				{
					string file = string.Format(TmpOutputFilePattern, System.IO.Path.GetFileName(this.Path));

					return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), file);
				}

				return null;
			}
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

		#region [ Methods ]

		public override string ToString()
		{
			return string.Format("{0} (addToProject = {1})",
				!string.IsNullOrEmpty(this.Path) ? System.IO.Path.GetFileName(this.Path) : "n.c.",
				this.AddToProject);
		}

		#endregion
	}
}

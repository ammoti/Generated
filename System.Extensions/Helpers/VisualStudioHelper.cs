// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;

	public static class VisualStudioHelper
	{
		#region [ Members ]

		private static readonly IDictionary<string, XmlDocument> _projectCache = new Dictionary<string, XmlDocument>();

		#endregion

		#region [ Public Methods ]

		/// <summary>
		/// Add a list of file to theirs Visual Studio projects.
		/// Each file should be link to its Visual Studio project.
		/// </summary>
		/// 
		/// <param name="files">
		/// Files to add the the Visual Studio projects.
		/// </param>
		/// 
		/// <param name="onFileAddedAction">
		/// Action to execute when a file is added.
		/// </param>
		/// 
		/// <returns>
		/// The number of added files.
		/// </returns>
		public static int AddToProject(IEnumerable<string> files, Action<string> onFileAddedAction = null)
		{
			int nbOfFilesAdded = 0;

			if (!files.IsNullOrEmpty())
			{
				foreach (var file in files)
				{
					string projectFilePath = GetProjectPath(file);

					if (string.IsNullOrEmpty(projectFilePath))
					{
						continue;
					}

					XmlDocument xmlProject = new XmlDocument();
					xmlProject.Load(projectFilePath);

					bool bFirst = false;

					if (!_projectCache.ContainsKey(projectFilePath))
					{
						_projectCache.Add(projectFilePath, xmlProject);

						bFirst = true;
					}

					xmlProject = _projectCache[projectFilePath];

					bool isAdded = AddToProject(file, projectFilePath, xmlProject);

					if (isAdded)
					{
						nbOfFilesAdded++;

						if (onFileAddedAction != null)
						{
							onFileAddedAction(file);
						}
					}

					if (!isAdded && bFirst)
					{
						_projectCache.Remove(projectFilePath);
					}
				}

				// Commit add operations
				foreach (string projectPath in _projectCache.Keys)
				{
					_projectCache[projectPath].Save(projectPath);
				}
			}

			return nbOfFilesAdded;
		}

		/// <summary>
		/// Determines the path of the project (.csproj or .sqlproj) from a given file.
		/// </summary>
		/// 
		/// <param name="filePath">
		/// The path of the file.
		/// </param>
		/// 
		/// <returns>
		/// The path of the project (.csproj or .sqlproj).
		/// </returns>
		public static string GetProjectPath(string filePath)
		{
			string projectFilePath = null;
			string currentPath = Path.GetDirectoryName(filePath);

			while (currentPath.Length > 3 /* C:\ */)
			{
				string[] csProjFiles = Directory.GetFiles(currentPath, "*.csproj");
				string[] sqlProjFiles = Directory.GetFiles(currentPath, "*.sqlproj");

				// This does not work if the directory contains more than 1 project file

				string tmpProjFile = null;

				if (csProjFiles.Length == 1) tmpProjFile = csProjFiles[0];
				if (sqlProjFiles.Length == 1) tmpProjFile = sqlProjFiles[0];

				if (!string.IsNullOrEmpty(tmpProjFile))
				{
					projectFilePath = Path.Combine(currentPath, tmpProjFile);
					break;
				}
				else
				{
					currentPath = Path.GetFullPath(Path.Combine(currentPath, ".."));
				}
			}

			return projectFilePath;
		}

		#endregion

		#region [ Private Methods ]

		private static bool AddToProject(string filePath, string projectFilePath, XmlDocument project)
		{
			const string ns = "http://schemas.microsoft.com/developer/msbuild/2003";

			string relativePath = filePath.Replace(Path.GetDirectoryName(projectFilePath), string.Empty);
			if (relativePath.StartsWith(@"\"))
			{
				relativePath = relativePath.Substring(1);
			}

			XmlNamespaceManager nsMgr = new XmlNamespaceManager(project.NameTable);
			nsMgr.AddNamespace("ms", ns);

			string nodeName = null;
			string itemGroupXPath_1 = null;
			string itemGroupXPath_2 = null;

			string extension = Path.GetExtension(relativePath).ToLower();
			switch (extension)
			{
				case ".cs":
					nodeName = "Compile";
					itemGroupXPath_1 = "//ms:Project/ms:ItemGroup/ms:Compile[ @Include = '{0}' ]";
					itemGroupXPath_2 = "//ms:Project/ms:ItemGroup[ count( ms:Compile ) > 0 ]";
					break;
				case ".sql":
					nodeName = "Build";
					itemGroupXPath_1 = "//ms:Project/ms:ItemGroup/ms:Build[ @Include = '{0}' ]";
					itemGroupXPath_2 = "//ms:Project/ms:ItemGroup[ count( ms:Build ) > 0 ]";
					break;
				default:
					nodeName = "Content";
					itemGroupXPath_1 = "//ms:Project/ms:ItemGroup/ms:Content[ @Include = '{0}' ]";
					itemGroupXPath_2 = "//ms:Project/ms:ItemGroup[ count( ms:Content ) > 0 ]";
					break;
			}

			XmlNodeList nodes = project.SelectNodes(string.Format(itemGroupXPath_1, relativePath), nsMgr);
			if (nodes.Count != 0)
			{
				return false; // Already attached
			}

			nodes = project.SelectNodes(itemGroupXPath_2, nsMgr);
			if (nodes.Count == 0)
			{
				// ItemGroup[ Compile | Content ] not found. Create it...

				XmlAttribute attr = project.CreateAttribute("Include");
				attr.Value = relativePath;

				XmlElement node = project.CreateElement(nodeName, ns);
				node.Attributes.Append(attr);

				XmlElement itemGroupNode = project.CreateElement("ItemGroup", ns);
				itemGroupNode.AppendChild(node);

				project.DocumentElement.AppendChild(itemGroupNode);
			}
			else
			{
				XmlAttribute xmlAttrInclude = project.CreateAttribute("Include");
				xmlAttrInclude.Value = relativePath;

				XmlElement xmlElement = project.CreateElement(nodeName, ns);
				xmlElement.Attributes.Append(xmlAttrInclude);

				nodes[0].AppendChild(xmlElement);
			}

			return true;
		}

		#endregion
	}
}

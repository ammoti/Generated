// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Threading;

	using Microsoft.SqlServer.Management.Smo;

	using LayerCake.Generator.Commons;

	public class Processor
	{
		#region [ Members ]

		private Stopwatch _stopwatch = null;

		#endregion

		#region [ Events ]

		public event ProcessMessageDelegate OnProcessMessage = null;

		public event ProcessProgressionDelegate OnProcessProgression = null;

		#endregion

		#region [ Constructor ]

		public Processor()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= OnCurrentDomainAssemblyResolve;
			AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainAssemblyResolve;

			this.Context = new ProcessorContext();
		}

		#endregion

		#region [ Events ]

		private Assembly OnCurrentDomainAssemblyResolve(object sender, ResolveEventArgs e)
		{
			if (e.RequestingAssembly == null)
			{
				return null;
			}

			AssemblyName assemblyName = new AssemblyName(e.Name);

			if (assemblyName.Name.EndsWith(".resources", StringComparison.InvariantCultureIgnoreCase) &&
			   !assemblyName.CultureName.EndsWith("neutral", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}

			// Try to load the dependencies located to the temp directory...

			string tmpPath = Path.Combine(Path.GetTempPath(), "LayerCake Generator", this.Context.ProjectName);

			FileInfo[] files = new DirectoryInfo(tmpPath).GetFiles(string.Format("{0}.*.dll", assemblyName.Name));
			FileInfo file = files.First(i => i.CreationTimeUtc == files.Max(j => j.CreationTimeUtc));

			string requiredAssemblyPath = Path.Combine(tmpPath, file.Name);

			ProxyDomain proxyDomain = new ProxyDomain();
			Assembly assembly = proxyDomain.LoadAssembly(requiredAssemblyPath, withPdbSymbols: true);

			return assembly;
		}

		private void OnFileGenerated(object sender, TextTemplatingEngineFileGeneratedEventArgs e)
		{
			if (e.IsAttached)
			{
				// For the 'Services' popup at the end of the process.

				string filePath = e.File.ToLowerInvariant();

				if (filePath.Contains(@"Services\".ToLowerInvariant()))
				{
					if (filePath.EndsWith("Service.generated.cs".ToLowerInvariant()))
					{
						this.GeneratedServices.Add(Path.GetFileName(e.File).Split(new char[] { '.' }).First());
					}
				}
			}
		}

		#endregion

		#region [ Public Methods ]

		/// <summary>
		/// Initializes the Processor.
		/// </summary>
		/// 
		/// <param name="configFilePath">
		/// Path of the configuration file to process.
		/// </param>
		public bool Initialize(string configFilePath)
		{
			bool isInitialized = false;

			do
			{
				this.PublishProcessMessage("Loading Environment...");

				#region [ Loading Config File ]

				this.PublishProcessMessage("Loading {0}...", Path.GetFileName(configFilePath));

				ProcessorContextLoadStatusEnum status = ProcessorContextLoadStatusEnum.None;

				try
				{
					status = this.Context.Load(configFilePath);

					#region [ BadFile ]

					const string token = "Configuration error";

					if (status == ProcessorContextLoadStatusEnum.BadFile)
					{
						this.PublishErrorMessage(
							"{0}: it seems that the '{1}' file is not a correct LayerCake Generator configuration file...",
							token, Path.GetFileName(configFilePath));

						break;
					}

					#endregion

					#region [ WithErrors ]

					if (status == ProcessorContextLoadStatusEnum.WithErrors)
					{
						foreach (string error in this.Context.Errors)
						{
							this.PublishErrorMessage("{0}: {1}", token, error);
						}

						break;
					}

					#endregion

					#region [ BadVersion ]

					if (status == ProcessorContextLoadStatusEnum.BadVersion)
					{
						string version = null;
						if (!string.IsNullOrEmpty(this.Context.Version))
						{
							version = string.Format(" v{0}", this.Context.Version);
						}

						this.PublishErrorMessage(
							"This project{0} is not compatible with the LayerCake Generator v{1}",
							version, GetExecutingAssemblyVersion());

						break;
					}

					#endregion
				}
				catch (Exception x)
				{
					this.PublishErrorMessage("Configuration error: {0}", x.GetFullMessage(false));

					break;
				}

				#endregion

				#region [ Loading SQL Server Context ]

				this.PublishProcessMessage("Loading SQL Server Context...");

				this.SmoContext = new SmoContext(
					this.Context.Database.Host, this.Context.Database.Name,
					this.Context.Database.User, this.Context.Database.Pass);

				try
				{
					this.SmoContext.LoadContext();
				}
				catch (Exception x)
				{
					this.PublishErrorMessage(
						"Failed while loading SQL Server Context -> {0}",
						x.GetFullMessage(false));

					this.PublishErrorMessage(
						"If login error then open {0} and report to the 'SQL Server Information' section.",
						Path.GetFileName(configFilePath));

					break;
				}

				this.PublishMessage(
					"SQL Server {0} v{1}, SMO v{2}",
					this.SmoContext.SqlServerYearVersion,
					this.SmoContext.SqlServerFullVersion,
					this.SmoContext.SmoVersion);

				if (this.SmoContext.SqlServerMajorVersion != "10" &&
					this.SmoContext.SqlServerMajorVersion != "11" &&
					this.SmoContext.SqlServerMajorVersion != "12")
				{
					this.PublishProcessMessage("Only Microsoft SQL Server 2014 (12.x), 2012 (11.x) and 2008 (10.x) are supported.");

					break;
				}

				#endregion

				#region [ Loading Database Table Definitions ]

				this.PublishProcessMessage("Loading Database Table Definitions...");

				bool hasErrors = false;

				this.TableNames = new List<string>();
				this.TableDependencies = new List<KeyValuePair<string, string>>();
				this.ExcludedTableNames = new List<string>();

				var reservedNames = new List<string> { "AUTHENTICATION", "COMMON", "STATE" };

				foreach (string tableName in this.SmoContext.TableNames)
				{
					if (reservedNames.Contains(tableName.ToUpperInvariant()))
					{
						this.PublishErrorMessage("The '{0}' table name is forbidden because it is reserved.", tableName);

						hasErrors = true;
					}

					if (tableName.ToUpperInvariant().EndsWith("_LOGS"))
						continue;

					// A table must have, at least:
					//  - a PK column,
					//  - another column.

					bool bHasPk = false;
					Table table = this.SmoContext.Tables.First(t => t.Name == tableName);

					if (table.Columns.Count < 2)
					{
						this.ExcludedTableNames.Add(tableName);
						continue;
					}

					foreach (Column column in table.Columns)
					{
						if (column.InPrimaryKey)
						{
							bHasPk = true;
							break;
						}
					}

					if (!bHasPk)
					{
						this.ExcludedTableNames.Add(tableName);
						continue;
					}

					this.LoadTableDependencies(tableName);

					this.TableNames.Add(tableName);
				}

				if (hasErrors)
				{
					break;
				}

				#endregion

				this.PublishProcessMessage("Environment successfully loaded.");

				isInitialized = true;
			}
			while (false);

			this.IsInitialized = isInitialized;

			return this.IsInitialized;
		}

		public void Execute(IProcessorBehavior behavior, ProcessorParameters parameters)
		{
			if (this.IsRunning)
			{
				ThrowException.Throw("The processor is already running");
			}

			do
			{
				#region [ Begin ]

				this.IsRunning = true;

				_stopwatch = new Stopwatch();
				_stopwatch.Start();

				if (this.CancelToken != null)
					this.CancelToken.Dispose();

				this.CancelToken = new CancellationTokenSource();
				this.GeneratedServices = new List<string>();

				this.HasErrors = false;

				this.PublishProcessMessage("Running Process ({0} mode)...", parameters.CompilationMode);

				#endregion

				#region [ Checking Assemblies Dependencies ]

				if (this.CancelToken.IsCancellationRequested)
					break;

				this.PublishProcessMessage("Checking Assemblies Dependencies...");

				bool bOk = true;

				string modelAssemblyPath = Path.Combine(this.Context.SrcDirectory,
					string.Format(@"{0}.Models", this.Context.ProjectName),
					string.Format(@"bin\{0}", parameters.CompilationMode),
					string.Format(@"{0}.Models.dll", this.Context.ProjectName));

				if (!File.Exists(modelAssemblyPath))
				{
					bOk = false;
				}

				if (bOk)
				{
					string businessAssemblyPath = Path.Combine(this.Context.SrcDirectory,
						string.Format(@"{0}.Business", this.Context.ProjectName),
						string.Format(@"bin\{0}", parameters.CompilationMode),
						string.Format(@"{0}.Business.dll", this.Context.ProjectName));

					if (!File.Exists(businessAssemblyPath))
					{
						bOk = false;
					}
				}

				if (!bOk)
				{
					this.PublishErrorMessage(
						"The whole solution must be compiled ({0} mode) before running the proccess.",
						parameters.CompilationMode);

					break;
				}

				#endregion

				#region [ Checking and Loading Culture Context ]

				if (this.CancelToken.IsCancellationRequested)
					break;

				this.PublishProcessMessage("Checking and Loading Culture Context...");

				string[] supportedCultures = DatabaseHelper.GetCultures(this.SmoContext);
				this.Context.Culture.SupportedCultures = supportedCultures;

				if (this.Context.Culture.SupportedCultures.Length == 0)
				{
					this.PublishErrorMessage("The 'Translation' table must define at least one culture!");

					break;
				}

				foreach (string culture in supportedCultures.Where(c => c.Length != 2))
				{
					this.PublishErrorMessage(
						"The '{0}' culture column from the 'Translation' table is not correct! Culture columns must be 2 characters long.",
						culture);

					break;
				}

				if (!supportedCultures.Contains(this.Context.Culture.Default))
				{
					this.PublishErrorMessage(
						"The default culture '{0}' is not supported ({1}).",
						this.Context.Culture.Default, string.Join(", ", supportedCultures));

					break;
				}

				#endregion

				#region [ Checking and Loading Database Structure ]

				if (this.CancelToken.IsCancellationRequested)
					break;

				this.PublishProcessMessage("Checking and Loading Database Structure...");

				this.ModelDescriptor = new ModelDescriptor(this.SmoContext, this.Context);

				string mdFile = null;

				try
				{
					mdFile = this.ModelDescriptor.Load(this.CancelToken.Token);
				}
				catch (Exception x)
				{
					this.PublishErrorMessage(
						"Failed while loading Database Structure (Table: '{1}' - Column: '{2}') -> {0}",
						x, this.ModelDescriptor.CurrentTable.Name, this.ModelDescriptor.CurrentColumn.Name);

					break;
				}

				if (this.CancelToken.IsCancellationRequested)
					break;

				if (this.ModelDescriptor.Errors.Count != 0)
				{
					foreach (var error in this.ModelDescriptor.Errors)
					{
						this.PublishErrorMessage(error.GetError());
					}

					break;
				}

				#endregion

				#region [ Checking Authentication Support ]

				if (this.CancelToken.IsCancellationRequested)
					break;

				var userTable = this.ModelDescriptor.Schema.Tables.FirstOrDefault(t => t.Name == this.Context.AuthenticationSupport.UserTableName);
				if (userTable == null)
				{
					this.PublishErrorMessage(
						"The '{0}' table declared in Context.AuthenticationSupport.UserTableName cannot be found in the database!",
						this.Context.AuthenticationSupport.UserTableName);

					break;
				}
				else
				{
					if (userTable.Columns.Count(c => c.Name == this.Context.AuthenticationSupport.UserTableIdentifiedColumnName) == 0)
					{
						this.PublishErrorMessage(
							"The '{0}' table must have a column named '{1}'!",
							this.Context.AuthenticationSupport.UserTableName,
							this.Context.AuthenticationSupport.UserTableIdentifiedColumnName);

						break;
					}

					// UserTableIdentifiedColumnName must have an UNIQUE constraint...
					if (userTable.Constraints == null || userTable.Constraints.Count(c => c.Columns.Any(i => i.Name == this.Context.AuthenticationSupport.UserTableIdentifiedColumnName)) == 0)
					{
						this.PublishErrorMessage(
							"The '{0}' column from the '{1}' table must have an UNIQUE constraint in order to be used as identifier column!",
							this.Context.AuthenticationSupport.UserTableIdentifiedColumnName,
							this.Context.AuthenticationSupport.UserTableName);


						break;
					}

					// UserTableName must define Password & Culture columns
					if (userTable.Columns.Count(c => c.CSharpPropertyName == "Password") == 0 || userTable.Columns.Count(c => c.CSharpPropertyName == "Culture") == 0)
					{
						this.PublishErrorMessage(
							"The '{0}' table must have '{0}_Password' and '{0}_Culture' columns!",
							this.Context.AuthenticationSupport.UserTableName);

						break;
					}
				}

				#endregion

				#region [ Loading Assemblies Dependencies ]

				if (this.CancelToken.IsCancellationRequested)
					break;

				this.PublishProcessMessage("Loading Assemblies Dependencies...");

				Assembly businessAssembly = null;

				try
				{
					string tmpDir = Path.Combine(Path.GetTempPath(), "LayerCake Generator", this.Context.ProjectName);
					if (!Directory.Exists(tmpDir))
					{
						Directory.CreateDirectory(tmpDir);
					}

					foreach (FileInfo file in new DirectoryInfo(tmpDir).GetFiles())
					{
						string extension = file.Extension.ToLowerInvariant();
						if (extension == ".dll" || extension == ".pdb")
						{
							try
							{
								file.Delete();
							}
							catch (UnauthorizedAccessException)
							{
								// Occurred when the assembly is already loaded in the AppDomain.CurrentDomain
							}
							catch (Exception)
							{
							}
						}
					}

					string guid = Guid.NewGuid().ToString();

					string assemblyPath = Path.GetDirectoryName(
						this.Context.GetAssemblyPath("Business", parameters.CompilationMode));

					if (!Directory.Exists(assemblyPath))
					{
						this.PublishErrorMessage("Cannot locate the path {0}", assemblyPath);

						break;
					}

					string businessAssemblyPath = null;
					ProxyDomain proxyDomain = new ProxyDomain();

					// Copy all DLL/PDB files from the Business\bin\Debug to the Temp...

					foreach (FileInfo file in new DirectoryInfo(assemblyPath).GetFiles())
					{
						string extension = file.Extension.ToLowerInvariant();
						if (extension == ".dll" || extension == ".pdb")
						{
							string copiedDllPath = Path.Combine(tmpDir, string.Format("{0}.{1}{2}", Path.GetFileNameWithoutExtension(file.Name), guid, extension));
							File.Copy(file.FullName, copiedDllPath);

							if (copiedDllPath.Contains(".Business") && Path.GetExtension(copiedDllPath).ToLowerInvariant() == ".dll")
							{
								businessAssemblyPath = copiedDllPath;
							}
						}
					}

					// Must be at the end once all the DLLs have been copied.
					// (cf. AppDomain Assembly Resolving (Program.cs, CurrentDomain_AssemblyResolve))...
					businessAssembly = proxyDomain.LoadAssembly(businessAssemblyPath);
				}
				catch (Exception x)
				{
					this.PublishErrorMessage(
						"Cannot load Assemblies Dependencies -> {0}",
						x.GetFullMessage(withExceptionType: true));

					break;
				}

				#endregion

				#region [ Loading Business Context ]

				if (this.CancelToken.IsCancellationRequested)
					break;

				this.PublishProcessMessage("Loading Business Context...");

				IList<string> errors;
				this.BusinessContext = new BusinessContext(businessAssembly);

				try
				{
					this.BusinessContext.Load(out errors);
				}
				catch (Exception x)
				{
					this.PublishErrorMessage("Cannot load the Business Context -> {0}", x.Message);

					break;
				}

				if (errors.Count != 0)
				{
					foreach (string error in errors)
					{
						this.PublishErrorMessage(error);
					}

					break;
				}

				#endregion

				#region [ Creating TextTemplating TextTemplatingEngine ]

				if (this.CancelToken.IsCancellationRequested)
					break;

				this.PublishProcessMessage("Creating TextTemplating Engine...");

				this.TextTemplatingEngine = new TextTemplatingEngine(
					this.Context.SolutionDirectory,
					this.Context.TemplateDirectory);

				this.TextTemplatingEngine.OnGeneratedFile += OnFileGenerated;

				#endregion

				#region [ Executing... ]

				behavior.Execute(this, parameters);

				#endregion
			}
			while (false);

			#region [ End ]

			_stopwatch.Stop();

			int nbFiles = this.TextTemplatingEngine != null ? this.TextTemplatingEngine.GeneratedFiles.Count() : 0;
			string summary = string.Format("Process Duration -> {0}secs ({1} files)", (int)_stopwatch.Elapsed.TotalSeconds, nbFiles);

			if (this.HasErrors)
			{
				this.PublishErrorMessage("Undo code modifications");
				this.PublishErrorMessage(summary);
			}
			else
			{
				if (this.CancelToken.IsCancellationRequested) // the process has been stopped -> do not commit (-> do not call LinkGeneratedFiles())
				{
					this.PublishErrorMessage("Undo code modifications"); // cancel user action
				}
				else
				{
					this.LinkGeneratedFiles();
				}

				this.PublishProcessMessage(summary);
			}

			this.Clear();

			this.IsRunning = false;

			#endregion
		}

		public void Stop()
		{
			if (this.IsRunning)
			{
				this.CancelToken.Cancel();
				this.IsRunning = false;
			}
		}

		#endregion

		#region [ Internal Methods ]

		internal void PublishMessage(string message, params object[] args)
		{
			if (this.OnProcessMessage != null)
			{
				this.OnProcessMessage(this,
					new ProcessMessageEventArgs(ProcessMessageType.Done, string.Format(message, args)));
			}
		}

		internal void PublishProcessMessage(string message, params object[] args)
		{
			if (this.OnProcessMessage != null)
			{
				this.OnProcessMessage(this,
					new ProcessMessageEventArgs(ProcessMessageType.Processing, string.Format(message, args)));
			}
		}

		internal void PublishErrorMessage(string message, params object[] args)
		{
			if (this.OnProcessMessage != null)
			{
				this.OnProcessMessage(this,
					new ProcessMessageEventArgs(ProcessMessageType.Error, string.Format(message, args)));
			}

			this.HasErrors = true;
		}

		internal void PublishProcessPercentage(double percentage)
		{
			if (this.OnProcessProgression != null)
			{
				this.OnProcessProgression(this, new ProcessProgressionEventArgs(percentage));
			}
		}

		internal void PublishProcessPercentage(int currentIndex, int totalIndex)
		{
			this.PublishProcessPercentage(currentIndex * 100 / (double)totalIndex);
		}

		internal string GetTemplateFilePathToProcess(ITemplateDefinition definition)
		{
			return Path.Combine(this.Context.TemplateDirectory, @"Projects", definition.TemplatePath);
		}

		internal string GetOutputFilePathToCreate(ITemplateDefinition definition, string name = null)
		{
			string outputPath = this.Context.SolutionDirectory;
			string outFileName = string.Format(definition.OutputFileNamePattern, name);

			string[] cutter = definition.TemplatePath.Split('\\');

			cutter[1] = string.Format("{0}.{1}", this.Context.ProjectName, cutter[1]);

			outputPath = Path.Combine(outputPath, Path.GetDirectoryName(string.Join(@"\", cutter)));

			return Path.Combine(outputPath, outFileName);
		}

		internal void Process(string t4Template, string outputFilePath, ITemplateDefinition definition, ProcessorParameters parameters)
		{
			var ttParams = new TextTemplatingParameters(
				t4Template,
				outputFilePath,
				definition.OverrideFileIfExists,
				definition.AddToProject);

			string tmpOutputFilePath;

			try
			{
				var state = this.TextTemplatingEngine.Process(ttParams, out tmpOutputFilePath);

				if (state == ProcessStateEnum.Processed)
				{
					// SQL scripts...

					if (definition.ExecuteSqlScript)
					{
						bool canExecuted = true;
						if (definition is TemplateDefinitions.Database.InsertAllStoredProceduresGeneratedSqlTemplateDefinition && !parameters.WithSqlProcedureIntegration)
						{
							canExecuted = false;
						}

						if (canExecuted)
						{
							this.PublishProcessMessage("Executing {0}...", Path.GetFileName(tmpOutputFilePath));

							try
							{
								SmoHelper.RunSqlScript(tmpOutputFilePath, this.SmoContext);
							}
							catch
							{
								// Create a copy of the tmpOutputFilePath file (for debug) because it will be deleted at the end of the whole process...
								FileHelper.TryCopy(tmpOutputFilePath, string.Concat(tmpOutputFilePath, ".err"), withOverwrite: true);

								throw;
							}
						}
					}
				}

				this.HasErrors |= state == ProcessStateEnum.Failed;
			}
			catch (Exception x)
			{
				this.PublishErrorMessage("Failed while processing {0} -> {1}", Path.GetFileName(t4Template), x.Message);
			}

			foreach (CompilerError error in this.TextTemplatingEngine.Errors)
			{
				this.PublishErrorMessage(
					"Template error: {0}({1},{2}): error {3}: {4}",
					Path.GetFileName(this.TextTemplatingEngine.Errors.First().FileName), error.Line, error.Column, error.ErrorNumber, error.ErrorText);
			}
		}

		#endregion

		#region [ Private Methods ]

		private void LoadTableDependencies(string tableName)
		{
			var table = this.SmoContext.Tables.First(i => i.Name == tableName);

			foreach (ForeignKey fkTable in table.ForeignKeys)
			{
				this.TableDependencies.Add(
					new KeyValuePair<string,string>(
						tableName, fkTable.ReferencedTable));
			}

			foreach (var t in this.SmoContext.Tables.Where(i => i.Name != tableName))
			{
				foreach (ForeignKey fkTable in t.ForeignKeys)
				{
					if (fkTable.ReferencedTable == tableName)
					{
						this.TableDependencies.Add(
							new KeyValuePair<string,string>(
								tableName, fkTable.Parent.Name));
					}
				}
			}
		}

		private void LinkGeneratedFiles()
		{
			if (this.TextTemplatingEngine.Errors.IsNullOrEmpty())
			{
				if (!this.TextTemplatingEngine.GeneratedFiles.IsNullOrEmpty())
				{
					if (this.TextTemplatingEngine.GeneratedFiles.Count(i => i.AddToProject) != 0)
					{
						this.PublishProcessMessage("Linking files to the projects...");

						this.TextTemplatingEngine.AttachGeneratedFiles();
					}
				}
			}
		}

		private void Clear()
		{
			if (this.TextTemplatingEngine != null)
			{
				this.TextTemplatingEngine.Clear();
			}
		}

		private static string GetExecutingAssemblyVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5);
		}

		#endregion

		#region [ Public Properties ]

		/// <summary>
		/// Gets the value indicating whether the process is initialized.
		/// </summary>
		public bool IsInitialized { get; private set; }

		/// <summary>
		/// Gets the value indicating whether the process is running.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// Gets the Processor Context.
		/// </summary>
		public ProcessorContext Context { get; private set; }

		/// <summary>
		/// Gets the table names from the database.
		/// </summary>
		public IList<string> TableNames { get; private set; }

		/// <summary>
		/// Gets the dependencies between tables.
		/// </summary>
		public IList<KeyValuePair<string, string>> TableDependencies { get; private set; }

		/// <summary>
		/// Gets theexcluded table names from the database.
		/// </summary>
		public IList<string> ExcludedTableNames { get; private set; }

		/// <summary>
		/// Gets the names of the new generated services.
		/// </summary>
		public IList<string> GeneratedServices { get; private set; }

		#endregion

		#region [ Internal Properties ]

		/// <summary>
		/// Gets the value indicating whether the process fails.
		/// </summary>
		internal bool HasErrors { get; private set; }

		internal TextTemplatingEngine TextTemplatingEngine { get; private set; }

		internal SmoContext SmoContext { get; private set; }

		internal ModelDescriptor ModelDescriptor { get; private set; }

		internal BusinessContext BusinessContext { get; private set; }

		internal CancellationTokenSource CancelToken { get; private set; }

		#endregion
	}
}

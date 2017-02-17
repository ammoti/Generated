// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;

	public class ProcessorContext
	{
		#region [ Constants ]

		private static readonly string SRC_DIRECTORY_NAME = "Src";
		private static readonly string TEMPLATES_DIRECTORY_NAME = "Templates";

		#endregion

		#region [ Properties ]

		public string Version { get; private set; }

		public string ProjectName { get; private set; }

		public string SolutionDirectory { get; private set; }

		public string TemplateDirectory
		{
			get
			{
				if (!string.IsNullOrEmpty(this.AdvancedOptions.TemplateRootPath))
				{
					return this.AdvancedOptions.TemplateRootPath;
				}

				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TEMPLATES_DIRECTORY_NAME);
			}
		}

		public string SrcDirectory
		{
			get { return Path.Combine(this.SolutionDirectory, SRC_DIRECTORY_NAME); }
		}

		public DatabaseContext Database { get; private set; }

		public WebServicesContext WebServices { get; private set; }

		public AuthenticationSupportContext AuthenticationSupport { get; private set; }

		public AdvancedOptionsContext AdvancedOptions { get; private set; }

		public CultureContext Culture { get; private set; }

		public IList<string> Errors { get; private set; }

		#endregion

		#region [ Constructor ]

		public ProcessorContext()
		{
			this.Errors = new List<string>();

			this.Database = new DatabaseContext();
			this.WebServices = new WebServicesContext();
			this.AuthenticationSupport = new AuthenticationSupportContext();
			this.AdvancedOptions = new AdvancedOptionsContext();
			this.Culture = new CultureContext();
		}

		#endregion

		#region [ Public Methods ]

		public ProcessorContextLoadStatusEnum Load(string jsonConfigFile)
		{
			bool isBadFile = false;

			this.SolutionDirectory = Path.GetDirectoryName(jsonConfigFile);

			var config = JsonConfigHelper.LoadJsonFile(jsonConfigFile);

			#region [ Common ]

			#region [ Config.Version ]

			string key = "Config.Version";

			this.Version = config[key] as string;

			#endregion

			#region [ Config.ProjectName ]

			key = "Config.ProjectName";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);

				isBadFile = true;
			}
			else
			{
				this.ProjectName = config[key] as string;
			}

			#endregion

			#region [ Config.SolutionDir ]

			key = "Config.SolutionDir";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				string solutionDir = GetPathValue(config[key] as string);

				if (solutionDir != ".")
				{
					if (!Path.IsPathRooted(solutionDir))
					{
						solutionDir = Path.Combine(this.SolutionDirectory, solutionDir);
					}

					this.SolutionDirectory = solutionDir;
				}
			}

			#endregion

			#endregion

			#region [ Database ]

			#region [ Config.Database.Host ]

			key = "Config.Database.Host";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.Database.Host = config[key] as string;
			}

			#endregion

			#region [ Config.Database.Name ]

			key = "Config.Database.Name";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.Database.Name = config[key] as string;
			}

			#endregion

			#region [ Config.Database.Username ]

			key = "Config.Database.Username";

			if (config.ContainsKey(key))
			{
				this.Database.User = config[key] as string;
			}

			#endregion

			#region [ Config.Database.Password ]

			key = "Config.Database.Password";

			if (config.ContainsKey(key))
			{
				this.Database.Pass = config[key] as string;
			}

			#endregion

			#endregion

			#region [ WebServices ]

			#region [ Config.WebServices.WithWcfSecurity ]

			key = "Config.WebServices.WithWcfSecurity";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.WebServices.WithWcfSecurity = GetBoolValue(config[key] as string, false);
			}

			#endregion

			#region [ Config.WebServices.StandardRootUrl ]

			key = "Config.WebServices.StandardRootUrl";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.WebServices.StandardRootUrl = config[key] as string;
			}

			#endregion

			#region [ Config.WebServices.SecureRootUrl ]

			key = "Config.WebServices.SecureRootUrl";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.WebServices.SecureRootUrl = config[key] as string;
			}

			#endregion

			#endregion

			#region [ AuthenticationSupport ]

			#region [ Config.AuthenticationSupport.UserTableName ]

			key = "Config.AuthenticationSupport.UserTableName";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.AuthenticationSupport.UserTableName = config[key] as string;
			}

			#endregion

			#region [ Config.AuthenticationSupport.UserTableIdentifiedColumnName ]

			key = "Config.AuthenticationSupport.UserTableIdentifiedColumnName";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.AuthenticationSupport.UserTableIdentifiedColumnName = config[key] as string;
			}

			#endregion

			#endregion

			#region [ AdvancedOptions ]

			#region [ Config.AdvancedOptions.WithNoLockOption ]

			key = "Config.AdvancedOptions.WithNoLockOption";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.AdvancedOptions.WithNoLockOption = GetBoolValue(config[key] as string, true);
			}

			#endregion

			#region [ Config.AdvancedOptions.WithEmitDefaultValueFalseOption ]

			key = "Config.AdvancedOptions.WithEmitDefaultValueFalseOption";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.AdvancedOptions.WithEmitDefaultValueFalseOption = GetBoolValue(config[key] as string, true);
			}

			#endregion

			#region [ Config.AdvancedOptions.DoNoExposeGeneratedMethodsOnWcfSide ]

			key = "Config.AdvancedOptions.DoNoExposeGeneratedMethodsOnWcfSide";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.AdvancedOptions.DoNoExposeGeneratedMethodsOnWcfSide = GetBoolValue(config[key] as string, false);
			}

			#endregion

			#region [ Config.AdvancedOptions.TemplateRootPath ]

			key = "Config.AdvancedOptions.TemplateRootPath";

			this.AdvancedOptions.TemplateRootPath = GetPathValue(config[key] as string);

			#endregion

			#endregion

			#region [ Culture ]

			#region [ Config.Culture.Default ]

			key = "Config.Culture.Default";

			if (!config.ContainsKey(key))
			{
				this.AddKeyNotFoundError(key);
			}
			else
			{
				this.Culture.Default = config[key] as string;
			}

			#endregion

			#endregion

			if (isBadFile)
			{
				return ProcessorContextLoadStatusEnum.BadFile;
			}

			if (this.Errors.Count != 0)
			{
				return ProcessorContextLoadStatusEnum.WithErrors;
			}

			#region [ Version Verification ]

			if (string.IsNullOrEmpty(this.Version))
			{
				return ProcessorContextLoadStatusEnum.BadVersion;
			}

			if (this.Version.Substring(0, 3) != Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 3))
			{
				return ProcessorContextLoadStatusEnum.BadVersion;
			}

			#endregion

			return ProcessorContextLoadStatusEnum.Ok;
		}

		public string GetAssemblyPath(string assemblyName, string compilationMode)
		{
			return Path.Combine(
				this.SrcDirectory,
				string.Format(@"{0}.{1}", this.ProjectName, assemblyName),
				string.Format(@"bin\{0}", compilationMode),
				string.Format(@"{0}.{1}.dll", this.ProjectName, assemblyName));
		}

		#endregion

		#region [ Private Methods ]

		private void AddKeyNotFoundError(string key)
		{
			this.Errors.Add(string.Format("key not found -> {0}", key));
		}

		private void AddKeyValueError(string key, string value)
		{
			this.Errors.Add(string.Format("bad value or format -> {0} (value = '{1}')", key, value));
		}

		private bool GetBoolValue(string value, bool defaultValue)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			if (value == "1" || string.Compare(value, "true", true) == 0)
			{
				return true;
			}

			if (value == "0" || string.Compare(value, "false", true) == 0)
			{
				return false;
			}

			return defaultValue;
		}

		private string GetPathValue(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}

			return value.Replace("\"", string.Empty);
		}

		#endregion
	}

	#region [ Context Classes ]

	[Serializable]
	public class DatabaseContext
	{
		public string Host { get; set; }

		public string Name { get; set; }

		private string _user = null;
		public string User
		{
			get { return _user; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_user = value;
				}
			}
		}

		private string _pass = null;
		public string Pass
		{
			get { return _pass; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_pass = value;
				}
			}
		}

		public string ConnectionString
		{
			get
			{
				string connectionString = null;

				if (!string.IsNullOrEmpty(this.User) && !string.IsNullOrEmpty(this.Pass))
				{
					connectionString = string.Format(@"Data Source={0};Initial Catalog={1};User Id={2};Password={3};",
						this.Host, this.Name, this.User, this.Pass);
				}
				else
				{
					connectionString = string.Format(@"Data Source={0};Initial Catalog={1};User ID=sa;Password=Objectx04081991;",
						this.Host, this.Name);
				}

				return connectionString;
			}
		}
	}

	[Serializable]
	public class WebServicesContext
	{
		public bool WithWcfSecurity { get; set; }

		public string StandardRootUrl { get; set; }

		public string SecureRootUrl { get; set; }
	}

	[Serializable]
	public class WCFContext
	{
		public bool DoNoExposeGeneratedMethods { get; set; }
	}

	[Serializable]
	public class AuthenticationSupportContext
	{
		public string UserTableName { get; set; }

		public string IdUserColumnName
		{
			get { return string.Format("Id{0}", this.UserTableName); }
		}

		public string UserTableIdentifiedColumnName { get; set; }
	}

	[Serializable]
	public class AdvancedOptionsContext
	{
		public AdvancedOptionsContext()
		{
			this.WithNoLockOption = true;
			this.WithEmitDefaultValueFalseOption = false;
			this.DoNoExposeGeneratedMethodsOnWcfSide = false;
		}

		public bool WithNoLockOption { get; set; }

		public bool WithEmitDefaultValueFalseOption { get; set; }

		public bool DoNoExposeGeneratedMethodsOnWcfSide { get; set; }

		public string TemplateRootPath { get; set; }
	}

	[Serializable]
	public class CultureContext
	{
		public string Default { get; set; }

		public string[] SupportedCultures { get; set; }
	}

	#endregion
}

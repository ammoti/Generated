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
	using System.Collections.Specialized;
	using System.IO;
	using System.Text;

	using Microsoft.VisualStudio.TextTemplating;

	[Serializable]
	class TextTemplatingEngineHost : ITextTemplatingEngineHost, ITextTemplatingSessionHost
	{
		#region [ Members ]

		private readonly StringDictionary _parameters = new StringDictionary();

		private string _solutionDir = null;
		private string _templateDir = null;

		#endregion

		#region [ Constructor & Initialize ]

		public TextTemplatingEngineHost(string solutionDir, string templateDir)
		{
			this.FileExtension = ".txt";
			this.FileEncoding = Encoding.UTF8;

			this.Errors = new CompilerErrorCollection();

			_solutionDir = solutionDir.EndsWith(@"\") ? solutionDir : solutionDir + @"\";
			_templateDir = templateDir.EndsWith(@"\") ? templateDir : templateDir + @"\"; ;

			this.AddParameter("SolutionDir", _solutionDir);
			this.AddParameter("TemplateDir", _templateDir);
		}

		#endregion

		#region [ ITextTemplatingEngineHost Implementation ]

		/// <summary>
		/// Gets or sets the path of the text template that is being processed.
		/// </summary>
		public string TemplateFile { get; set; }

		/// <summary>
		/// Gets or sets the extension of the generated text output file.
		/// The host can provide a default by setting the value of the field here. The engine can change this based on the optional output directive if the user specifies it in the text template.
		/// </summary>
		public string FileExtension { get; private set; }

		/// <summary>
		/// Gets or sets the encoding of the generated text output file.
		/// The host can provide a default by setting the value of the field here. The engine can change this based on the optional output directive if the user specifies it in the text template.
		/// </summary>
		public Encoding FileEncoding { get; private set; }

		/// <summary>
		/// These are the errors that occur when the engine processes a template.
		/// The engine passes the errors to the host when it is done processing and the host can decide how to display them. For example, the host can display the errors in the UI or write them to a file.
		/// </summary>
		public CompilerErrorCollection Errors { get; private set; }

		/// <summary>
		/// Gets the standard assembly references.
		/// The engine will use these references when compiling and executing the generated transformation class.
		/// </summary>
		public IList<string> StandardAssemblyReferences
		{
			get
			{
				return new string[]
                {
                    typeof(System.Uri).Assembly.Location,
                    typeof(System.Collections.ArrayList).Assembly.Location,
                    typeof(System.Data.Constraint).Assembly.Location,
                    typeof(System.Linq.Enumerable).Assembly.Location,
                    typeof(System.Windows.Forms.MessageBox).Assembly.Location,
                    typeof(Microsoft.VisualStudio.TextTemplating.IDebugTextTemplatingEngine).Assembly.Location
                };
			}
		}

		/// <summary>
		/// Gets the standard imports or using statements.
		/// The engine will add these statements to the generated transformation class.
		/// </summary>
		public IList<string> StandardImports
		{
			get
			{
				return new string[]
                {
                    "System",
                    "System.Collections",
                    "System.Collections.Generic",
                    "System.Data",
                    "System.Linq",
                };
			}
		}

		/// <summary>
		/// Acquires the text that corresponds to a request to include a partial text template file.
		/// </summary>
		/// 
		/// <param name="requestFileName">
		/// The name of the partial text template file to acquire.
		/// </param>
		/// 
		/// <param name="content">
		/// A System.String that contains the acquired text or System.String.Empty if the file could not be found.
		/// </param>
		/// 
		/// <param name="location">
		/// A System.String that contains the location of the acquired text.
		/// If the host searches the registry for the location of include files or if the host searches
		/// multiple locations by default, the host can return the final path of the  include file in this parameter.
		/// The host can set the location to System.String.Empty if the file could not be found or if the host is not file-system based.
		/// </param>
		/// 
		/// <returns>
		/// True to indicate that the host was able to acquire the text; otherwise, false.
		/// </returns>
		public bool LoadIncludeText(string requestFileName, out string content, out string location)
		{
			content = string.Empty;
			location = string.Empty;

			do
			{
				location = requestFileName;
				if (File.Exists(location))
				{
					break;
				}

				location = Path.GetFullPath(Path.Combine(_templateDir, requestFileName));
				if (File.Exists(location))
				{
					break;
				}

				location = Path.GetFullPath(Path.Combine(_templateDir, "Commons", requestFileName));
				if (File.Exists(location))
				{
					break;
				}

				location = string.Empty; // File not found
			}
			while (false);

			if (!string.IsNullOrEmpty(location))
			{
				content = File.ReadAllText(location);
			}

			return content != null;
		}

		/// <summary>
		/// Allows a host to provide additional information about the location of an assembly.
		/// </summary>
		/// 
		/// <param name="assemblyReference">
		/// The assembly to resolve.
		/// </param>
		/// 
		/// <returns>
		/// A System.String that contains the specified assembly reference or the specified assembly reference with additional information.
		/// </returns>
		public string ResolveAssemblyReference(string assemblyReference)
		{
			// If the argument is the fully qualified path of an existing file, then we are done.

			if (File.Exists(assemblyReference))
			{
				return assemblyReference;
			}

			// Maybe the assembly is in the same folder as the text template that called the directive.

			string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
			if (File.Exists(candidate))
			{
				return candidate;
			}

			candidate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyReference);
			if (File.Exists(candidate))
			{
				return candidate;
			}

			return string.Empty;
		}

		/// <summary>
		/// Returns the type of a directive processor, given its friendly name.
		/// </summary>
		/// 
		/// <param name="processorName">
		/// The name of the directive processor to be resolved.
		/// </param>
		/// 
		/// <returns>
		/// The System.Type of the directive processor.
		/// </returns>
		public Type ResolveDirectiveProcessor(string processorName)
		{
			// This host will not resolve any specific processors.

			// Check the processor name, and if it is the name of a processor the 
			// host wants to support, return the type of the processor.
			//---------------------------------------------------------------------
			if (string.Compare(processorName, "XYZ", StringComparison.OrdinalIgnoreCase) == 0)
			{
				//return typeof();
			}

			// This can be customized to search specific paths for the file,
			// or to search the GAC.



			// If the directive processor cannot be found, throw an error.
			ThrowException.Throw("Directive Processor not found");

			return null; // for compilation only
		}

		/// <summary>
		/// Resolves the value of a parameter for a directive processor if the parameter is not specified in the template text.
		/// </summary>
		/// 
		/// <param name="directiveId">
		/// The ID of the directive call to which the parameter belongs.
		/// This ID disambiguates repeated calls to the same directive from the same text template.
		/// </param>
		/// 
		/// <param name="processorName">
		/// The name of the directive processor to which the directive belongs.
		/// </param>
		/// 
		/// <param name="parameterName">
		/// The name of the parameter to be resolved.
		/// </param>
		/// 
		/// <returns>
		/// A System.String that represents the resolved parameter value.
		/// </returns>
		public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
		{
			if (directiveId == null)
			{
				ThrowException.ThrowArgumentNullException("the directiveId cannot be null");
			}

			if (processorName == null)
			{
				ThrowException.ThrowArgumentNullException("the processorName cannot be null");
			}

			if (parameterName == null)
			{
				ThrowException.ThrowArgumentNullException("the parameterName cannot be null");
			}

			if (_parameters.ContainsKey(parameterName))
			{
				return _parameters[parameterName];
			}

			return string.Empty;
		}

		/// <summary>
		/// Tells the host the file name extension that is expected for the generated text output.
		/// </summary>
		/// 
		/// <param name="extension">
		/// The file name extension for the generated text output.
		/// </param>
		public void SetFileExtension(string extension)
		{
			this.FileExtension = extension;
		}

		/// <summary>
		/// Tells the host the encoding that is expected for the generated text output.
		/// </summary>
		/// 
		/// <param name="encoding">
		/// The encoding for the generated text output.
		/// </param>
		/// 
		/// <param name="fromOutputDirective">
		/// True to indicate that the user specified the encoding in the encoding parameter of the output directive.
		/// </param>
		public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
		{
			this.FileEncoding = encoding;
		}

		/// <summary>
		/// Receives a collection of errors and warnings from the transformation engine.
		/// </summary>
		/// 
		/// <param name="errors">
		/// The System.CodeDom.Compiler.CompilerErrorCollection being passed to the host from the engine.
		/// </param>
		public void LogErrors(CompilerErrorCollection errors)
		{
			this.Errors = errors;
		}

		/// <summary>
		/// Provides an application domain to run the generated transformation class.
		/// </summary>
		/// 
		/// <param name="content">
		/// The contents of the text template file to be processed.
		/// </param>
		/// 
		/// <returns>
		/// An System.AppDomain that compiles and executes the generated transformation class.
		/// </returns>
		public AppDomain ProvideTemplatingAppDomain(string content)
		{
			return AppDomain.CurrentDomain;

			// This host will provide a new application domain each time the 
			// engine processes a text template.
			// -------------------------------------------------------------

			//return AppDomain.CreateDomain("TextTemplatingAppDomain");

			// This could be changed to return the current appdomain, but new 
			// assemblies are loaded into this AppDomain on a regular basis.
			// If the AppDomain lasts too long, it will grow indefintely, 
			// which might be regarded as a leak.

			// This could be customized to cache the application domain for 
			// a certain number of text template generations (for example, 10).

			// This could be customized based on the contents of the text 
			// template, which are provided as a parameter for that purpose.
		}

		/// <summary>
		/// Called by the TextTemplatingEngine to ask for the value of a specified option.
		/// Return null if you do not know.
		/// </summary>
		/// 
		/// <param name="optionName">
		/// The name of an option.
		/// </param>
		/// 
		/// <returns>
		/// Null to select the default value for this option.
		/// Otherwise, an appropriate value for the option.
		/// </returns>
		public object GetHostOption(string optionName)
		{
			return optionName;
		}

		/// <summary>
		/// Allows a host to provide a complete path, given a file name or relative path.
		/// </summary>
		/// 
		/// <param name="path">
		/// The path to complete.
		/// </param>
		/// 
		/// <returns>
		/// A System.String that contains an absolute path.
		/// </returns>
		public string ResolvePath(string path)
		{
			return path;
		}

		#endregion

		#region [ ITextTemplatingSessionHost Implementation ]

		private ITextTemplatingSession _session = new TextTemplatingSession();

		/// <summary>
		/// Create a Session object that can be used to transmit information into a template.
		//  The new Session becomes the current Session.
		/// </summary>
		/// 
		/// <returns>
		/// A new session.
		/// </returns>
		public ITextTemplatingSession CreateSession()
		{
			return _session;
		}

		/// <summary>
		/// Gets or sets the current Session.
		/// </summary>
		public ITextTemplatingSession Session
		{
			get { return _session; }
			set { _session = value; }
		}

		#endregion

		#region [ Custom Methods ]

		public void AddParameter(string parameterName, string parameterValue)
		{
			if (string.IsNullOrEmpty(parameterName))
			{
				ThrowException.ThrowArgumentNullException("parameterName");
			}

			if (string.IsNullOrEmpty(parameterValue))
			{
				ThrowException.ThrowArgumentNullException("parameterValue");
			}

			if (_parameters.ContainsKey(parameterName))
			{
				_parameters[parameterName] = parameterValue;
			}
			else
			{
				_parameters.Add(parameterName, parameterValue);
			}
		}

		public void ClearParameters()
		{
			_parameters.Clear();
		}

		#endregion
	}
}

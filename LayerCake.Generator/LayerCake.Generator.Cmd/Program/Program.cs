// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Cmd
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using LayerCake.Generator.Commons;

	class Program
	{
		#region [ Members ]

		private static string _compilationMode = null;

		private static string _configFilePath = null;

		private static bool _withSqlProcedureIntegration = true;

		private static IList<string> _selectedTableNames = null;

		private static bool _withErrors = false;

		#endregion

		static void Main(string[] args)
		{
			ProgramHelper.ShowHeader();

			bool bCmdHandled = false;

			if (args != null)
			{
				if (args.Length > 1)
				{
					bCmdHandled = true;

					_compilationMode = args[0].ToUpperInvariant();
					if (_compilationMode != "DEBUG" && _compilationMode != "RELEASE")
					{
						ProgramHelper.WriteError("Parameter 1 -> invalid argument!");
						Environment.Exit(ProgramExitCodes.InvalidArguments);
					}

					_configFilePath = args[1];
					if (!File.Exists(_configFilePath))
					{
						ProgramHelper.WriteError("Cannot find the config file ({0})", _configFilePath);
						Environment.Exit(ProgramExitCodes.ConfigFileNotFound);
					}

					if (args.Length > 2)
					{
						if (!bool.TryParse(args[2], out _withSqlProcedureIntegration))
						{
							ProgramHelper.WriteError("Parameter 3 -> invalid argument!");
							Environment.Exit(ProgramExitCodes.InvalidArguments);
						}
					}

					if (args.Length > 3)
					{
						_selectedTableNames = new List<string>(args.Skip(3));
					}

					Program.Process();

					Console.WriteLine();
				}
			}

			if (!bCmdHandled)
			{
				ProgramHelper.ShowHelp();
			}

			if (_withErrors)
			{
				Environment.Exit(ProgramExitCodes.ProcessException);
			}

			Environment.Exit(ProgramExitCodes.Ok);
		}

		private static void Process()
		{
			ProgramHelper.WriteAction("Processing {0}...", Program.ShortConfigFilePath);
			Console.WriteLine();

			Processor processor = new Processor();
			processor.OnProcessMessage += OnProcessorProcessMessage;

			try
			{
				if (processor.Initialize(_configFilePath))
				{
					if (_selectedTableNames == null)
					{
						_selectedTableNames = processor.TableNames;
					}

					processor.Execute(new FullProcessorBehavior(), new ProcessorParameters
					{
						WithSqlProcedureIntegration = _withSqlProcedureIntegration,
						CompilationMode = _compilationMode,
						TableNames = _selectedTableNames.ToArray()
					});
				}
			}
			catch (Exception x)
			{
				_withErrors = true;

				Console.WriteLine();

				ProgramHelper.WriteError("Error while processing: {0}", x.GetFullMessage(withExceptionType: true));
				ProgramHelper.WriteError(x.StackTrace);
			}
		}

		private static void OnProcessorProcessMessage(object sender, ProcessMessageEventArgs e)
		{
			string message = e.Message;

			if (e.Type == ProcessMessageType.Processing ||
				e.Type == ProcessMessageType.Done)
			{
				message = string.Format("[{0}] >> {1}", DateTime.Now.ToString("HH:mm:ss"), message);
				Console.WriteLine(message);
			}
			else if (e.Type == ProcessMessageType.Error)
			{
				message = string.Format("[{0}] ** {1}", DateTime.Now.ToString("HH:mm:ss"), message);

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(message);
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

		private static string ShortConfigFilePath
		{
			get
			{
				string[] cutter = _configFilePath.Split(new char[] { '\\' });
				return string.Format(@"{0}\..\{1}", cutter[0], cutter[cutter.Length - 1]);
			}
		}
	}
}

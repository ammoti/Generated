// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Cmd
{
	using System;
	using System.Reflection;

	using LayerCake.Generator.Commons;

	static class ProgramHelper
	{
		public static void WriteAction(string action, params object[] args)
		{
			action = string.Format(action, args);

			Console.ForegroundColor = ConsoleColor.Yellow;

			Console.WriteLine(string.Format(">> {0}", action));

			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void WriteError(string error, params object[] args)
		{
			error = string.Format(error, args);

			Console.ForegroundColor = ConsoleColor.Red;

			Console.WriteLine(string.Format("** {0}", error));

			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void ShowHeader()
		{
			Console.ForegroundColor = ConsoleColor.Green;

			Console.WriteLine("LayerCake.Generator v{0}", ProgramHelper.ApplicationVersion);
			Console.WriteLine("Copyright (c) 2012, {0} LayerCake Generator", DateTime.Now.Year);
			Console.WriteLine("http://www.layercake-generator.net");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void ShowHelp()
		{
			Console.WriteLine("Usage");
			Console.WriteLine();

			Console.WriteLine("1. Full Process");
			Console.WriteLine("   {0} [Debug|Release] <ConfigFilePath>", ProgramHelper.ApplicationName);
			Console.WriteLine();

			Console.WriteLine("-> Example");
			Console.WriteLine("   {0} Debug \"C:\\..\\Project.config\"", ProgramHelper.ApplicationName);
			Console.WriteLine();

			Console.WriteLine("2. Process without Sql Procedure Integration");
			Console.WriteLine("   {0} [Debug|Release] <ConfigFilePath> false", ProgramHelper.ApplicationName);
			Console.WriteLine();

			Console.WriteLine("-> Example");
			Console.WriteLine("   {0} Release \"C:\\..\\Project.config\" false", ProgramHelper.ApplicationName);
			Console.WriteLine();

			Console.WriteLine("3. Process with Sql Procedure Integration and a set of tables");
			Console.WriteLine("   {0} [Debug|Release] <ConfigFilePath> true [Table1] [Table2] ...", ProgramHelper.ApplicationName);
			Console.WriteLine();

			Console.WriteLine("-> Example");
			Console.WriteLine("   {0} Release \"C:\\..\\Project.config\" true Product Order", ProgramHelper.ApplicationName);
		}

		public static string ApplicationName
		{
			get
			{
				return Assembly.GetExecutingAssembly().ManifestModule.ToString();
			}
		}

		public static string ApplicationVersion
		{
			get
			{
				return GlobalAssemblyInfo.AssemblyVersion.Substring(0, 5);
			}
		}
	}
}

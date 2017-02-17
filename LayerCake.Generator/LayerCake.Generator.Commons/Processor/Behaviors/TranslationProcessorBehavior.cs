// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System.Collections.Generic;
	using System.IO;

	public class TranslationProcessorBehavior : IProcessorBehavior
	{
		#region [ IProcessorBehavior Implementation ]

		public void Execute(Processor processor, ProcessorParameters parameters)
		{
			IList<ITemplateDefinition> definitions = new List<ITemplateDefinition>
			{
				new TemplateDefinitions.Database.TranslationInitializationGeneratedSqlTemplateDefinition(), // Must be the first one!
				new TemplateDefinitions.Core.TranslationEnumGeneratedCsTemplateDefinition()
			};

			#region [ Computing the number of files to create ]

			int totalFileNumber = 0;
			int currentFileNumber = 0;

			foreach (var definition in definitions)
			{
				totalFileNumber++;
			}

			#endregion

			foreach (var definition in definitions)
			{
				if (processor.CancelToken.IsCancellationRequested)
					return;

				processor.PublishProcessPercentage(++currentFileNumber, totalFileNumber);

				if (definition.CanBeExecuted(processor.Context))
				{
					string t4Template = processor.GetTemplateFilePathToProcess(definition);
					string outputFilePath = processor.GetOutputFilePathToCreate(definition);

					if (!definition.OverrideFileIfExists && File.Exists(outputFilePath))
					{
						continue;
					}

					processor.PublishProcessMessage("Processing {0}...", definition.TemplatePath);

					var p = new Dictionary<string, object>();

					p.Add("Context", new TextTemplatingProcessContext
					{
						ProcessorContext = processor.Context,
						SmoContext = processor.SmoContext,
						Schema = processor.ModelDescriptor.Schema
					});

					definition.AddParameters(processor.Context, p);
					processor.TextTemplatingEngine.SetParameters(p);

					processor.Process(t4Template, outputFilePath, definition, new ProcessorParameters
					{
						WithSqlProcedureIntegration = true, // Always execute *.generated.sql files,
						CompilationMode = parameters.CompilationMode
					});
				}
			}
		}

		#endregion
	}
}

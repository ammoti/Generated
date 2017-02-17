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
	using System.Linq;

	public class CodeRefProcessorBehavior : IProcessorBehavior
	{
		#region [ IProcessorBehavior Implementation ]

		public void Execute(Processor processor, ProcessorParameters parameters)
		{
			IList<ITemplateDefinition> definitions = new List<ITemplateDefinition>
			{
				new TemplateDefinitions.Models.EntityGeneratedCsTemplateDefinition(),
			};

			#region [ Computing the number of files to create ]

			int totalFileNumber = 0;
			int currentFileNumber = 0;

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.OneByTable))
			{
				foreach (var table in processor.ModelDescriptor.Schema.Tables /* All tables */)
				{
					totalFileNumber++;
				}
			}

			#endregion

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.OneByTable))
			{
				foreach (var table in processor.ModelDescriptor.Schema.Tables /* All tables */)
				{
					if (processor.CancelToken.IsCancellationRequested)
						return;

					processor.PublishProcessPercentage(++currentFileNumber, totalFileNumber);

					if (definition.CanBeExecuted(processor.Context, table))
					{
						string t4Template = processor.GetTemplateFilePathToProcess(definition);
						string outputFilePath = processor.GetOutputFilePathToCreate(definition, table.Name);

						if (!definition.OverrideFileIfExists && File.Exists(outputFilePath))
						{
							continue;
						}

						processor.PublishProcessMessage("Processing {0} (Table = {1})...", definition.TemplatePath, table.Name);

						var p = new Dictionary<string, object>();

						p.Add("Context", new TextTemplatingProcessContext
						{
							ProcessorContext = processor.Context,
							Schema = processor.ModelDescriptor.Schema,
							Table = table
						});

						definition.AddParameters(processor.Context, p);
						processor.TextTemplatingEngine.SetParameters(p);

						processor.Process(t4Template, outputFilePath, definition, parameters);
					}
				}
			}
		}

		#endregion
	}
}
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
	using System.Linq;

	public class FullProcessorBehavior : IProcessorBehavior
	{
		#region [ IProcessorBehavior Implementation ]

		public void Execute(Processor processor, ProcessorParameters parameters)
		{
			if (processor.CancelToken.IsCancellationRequested)
				return;

			processor.PublishProcessMessage("Loading T4 Template Definitions...");

			IList<ITemplateDefinition> definitions = null;

			try
			{
				definitions = TemplateDefinitionHelper.GetDefinitions();
			}
			catch (Exception x)
			{
				processor.PublishErrorMessage("Failed while loading T4 Template Definitions -> {0}", x.Message);

				return;
			}

			processor.PublishMessage("{0} T4 Template Definitions loaded.", definitions.Count);

			#region [ Computing the number of files to create ]

			int totalFileNumber = 0;
			int currentFileNumber = 0;

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.OneByTable))
			{
				foreach (var table in processor.ModelDescriptor.Schema.Tables.Where(
					i => parameters.TableNames.Contains(i.Name, new StringIgnoreCaseEqualityComparer())))
				{
					totalFileNumber++;
				}
			}

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.OneByBusiness))
			{
				foreach (var bcInfo in processor.BusinessContext.BusinessClasses)
				{
					totalFileNumber++;
				}
			}

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.Normal))
			{
				totalFileNumber++;
			}
            
            #endregion

            processor.PublishProcessMessage("Executing T4 Templates...");

			#region [ OneByTable Section ]

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.OneByTable))
			{
				foreach (var table in processor.ModelDescriptor.Schema.Tables.Where(
					i => parameters.TableNames.Contains(i.Name, new StringIgnoreCaseEqualityComparer())))
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
							Table = table,
							BusinessClass = processor.BusinessContext.GetBusinessClass(table.Name)
						});

						definition.AddParameters(processor.Context, p);
						processor.TextTemplatingEngine.SetParameters(p);

						processor.Process(t4Template, outputFilePath, definition, parameters);
					}
				}
			}

			#endregion

			#region [ OneByBusiness Section ]

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.OneByBusiness))
			{
				foreach (BusinessClassInfo bcInfo in processor.BusinessContext.BusinessClasses)
				{
					if (processor.CancelToken.IsCancellationRequested)
						return;

					processor.PublishProcessPercentage(++currentFileNumber, totalFileNumber);

					if (definition.CanBeExecuted(processor.Context, new object[] { processor.ModelDescriptor, bcInfo.Name }))
					{
						string t4Template = processor.GetTemplateFilePathToProcess(definition);
						string outputFilePath = processor.GetOutputFilePathToCreate(definition, bcInfo.Name);

						if (!definition.OverrideFileIfExists && File.Exists(outputFilePath))
						{
							continue;
						}

						processor.PublishProcessMessage("Processing {0} (Business = {1})...", definition.TemplatePath, bcInfo.Name);

						var p = new Dictionary<string, object>();

						p.Add("Context", new TextTemplatingProcessContext
						{
							ProcessorContext = processor.Context,
							Schema = processor.ModelDescriptor.Schema,
							Table = new ModelDescriptorSchemaTable { Name = bcInfo.Name },
							BusinessClass = processor.BusinessContext.GetBusinessClass(bcInfo.Name)
						});

						definition.AddParameters(processor.Context, p);
						processor.TextTemplatingEngine.SetParameters(p);

						processor.Process(t4Template, outputFilePath, definition, parameters);
					}
				}
			}

			#endregion

			#region [ Normal Section ]

			foreach (var definition in definitions.Where(i => i.Type == TemplateDefinitionType.Normal))
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
						Schema = processor.ModelDescriptor.Schema,
						BusinessClasses = processor.BusinessContext.BusinessClasses
					});

					definition.AddParameters(processor.Context, p);
					processor.TextTemplatingEngine.SetParameters(p);

					processor.Process(t4Template, outputFilePath, definition, parameters);
				}
			}

            #endregion
        }

		#endregion
	}
}

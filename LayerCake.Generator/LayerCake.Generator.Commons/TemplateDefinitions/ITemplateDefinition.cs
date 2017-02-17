// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System.Collections.Generic;

	using LayerCake.Generator.Commons;

	public interface ITemplateDefinition
	{
		#region [ Properties ]

		string Guid { get; }

		string TemplatePath { get; }

		TemplateDefinitionType Type { get; }

		string OutputFileNamePattern { get; }

		bool OverrideFileIfExists { get; }

		bool AddToProject { get; }

		bool ExecuteSqlScript { get; }

		#endregion

		#region [ Methods ]

		void AddParameters(ProcessorContext context, IDictionary<string, object> parameters);

		bool CanBeExecuted(ProcessorContext context, object parameter = null);

		#endregion
	}
}

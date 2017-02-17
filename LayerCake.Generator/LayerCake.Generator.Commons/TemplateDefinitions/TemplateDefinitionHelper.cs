// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
    using System.Collections.Generic;
    using TDBusinesss = LayerCake.Generator.Commons.TemplateDefinitions.Business;
    using TDContracts = LayerCake.Generator.Commons.TemplateDefinitions.Contracts;
    using TDCore = LayerCake.Generator.Commons.TemplateDefinitions.Core;
    using TDCrud = LayerCake.Generator.Commons.TemplateDefinitions.Crud;
    using TDDatabase = LayerCake.Generator.Commons.TemplateDefinitions.Database;
    using TDModels = LayerCake.Generator.Commons.TemplateDefinitions.Models;
    using TDServices = LayerCake.Generator.Commons.TemplateDefinitions.Services;
    using TDTests = LayerCake.Generator.Commons.TemplateDefinitions.Tests;
    using TDWebServices = LayerCake.Generator.Commons.TemplateDefinitions.WebServices;

    public static class TemplateDefinitionHelper
    {
        public static IList<ITemplateDefinition> GetDefinitions()
        {
            var definitions = new List<ITemplateDefinition>();

            // Order ITemplateDefinition instances for the process...

            #region [ Models Templates Definitions ]

            definitions.Add(new TDModels.IEntityGeneratedCsTemplateDefinition());
            definitions.Add(new TDModels.EntityGeneratedCsTemplateDefinition());
            definitions.Add(new TDModels.EntityCustomCsTemplateDefinition());
            definitions.Add(new TDModels.EntityBaseGeneratedCsTemplateDefinition());
            definitions.Add(new TDModels.MemberContextGeneratedCsTemplateDefinition());

            #endregion

            #region [ Crud Templates Definitions ]

            definitions.Add(new TDCrud.CrudGeneratedCsTemplateDefinition());
            definitions.Add(new TDCrud.CrudCustomCsTemplateDefinition());

            #endregion

            #region [ Database Templates Definitions ]

            definitions.Add(new TDDatabase.CountGeneratedSqlTemplateDefinition());
            definitions.Add(new TDDatabase.DeleteGeneratedSqlTemplateDefinition());
            definitions.Add(new TDDatabase.GetGeneratedSqlTemplateDefinition());
            definitions.Add(new TDDatabase.SaveGeneratedSqlTemplateDefinition());
            definitions.Add(new TDDatabase.SearchGeneratedSqlTemplateDefinition());
            definitions.Add(new TDDatabase.SetLockGeneratedSqlTemplateDefinition());
            definitions.Add(new TDDatabase.TranslationInitializationGeneratedSqlTemplateDefinition());
            definitions.Add(new TDDatabase.InsertAllStoredProceduresGeneratedSqlTemplateDefinition());

            #endregion

            #region [ Business Templates Definitions ]

            definitions.Add(new TDBusinesss.BusinessCustomCsTemplateDefinition());

            #endregion

            #region [ Contracts Templates Definitions ]

            definitions.Add(new TDContracts.ContractGeneratedCsTableTemplateDefinition());
            definitions.Add(new TDContracts.ContractGeneratedCsBusinessTemplateDefinition());

            #endregion

            #region [ Services Templates Definitions ]

            definitions.Add(new TDServices.ServiceGeneratedCsBusinessTemplateDefinition());
            definitions.Add(new TDServices.ServiceGeneratedCsTableTemplateDefinition());
            definitions.Add(new TDServices.ServiceContextGeneratedCsTemplateDefinition());

            #endregion

            #region [ WebServices Templates Definitions ]

            definitions.Add(new TDWebServices.ServiceModelServicesGeneratedConfigTemplateDefinition());
            definitions.Add(new TDWebServices.ServiceSvcBusinessTemplateDefinition());
            definitions.Add(new TDWebServices.ServiceSvcTableTemplateDefinition());

            #endregion

            #region [ Core Templates Definitions ]

            definitions.Add(new TDCore.LanguageCollectionGeneratedCsTemplateDefinition());
            definitions.Add(new TDCore.CulturesGeneratedCsTemplateDefinition());
            definitions.Add(new TDCore.TranslationEnumGeneratedCsTemplateDefinition());

            #endregion

            #region [ Tests Templates Definitions ]

            definitions.Add(new TDTests.LanguageCollectionTestsGeneratedCsTemplateDefinition());
            definitions.Add(new TDTests.ServiceModelClientGeneratedConfigTemplateDefinition());
            definitions.Add(new TDTests.ModelTestsGeneratedCsTemplateDefinition());
            definitions.Add(new TDTests.ModelTestsCustomCsTemplateDefinition());
            definitions.Add(new TDTests.ServiceTestsGeneratedCsTemplateDefinition());
            definitions.Add(new TDTests.ServiceTestsCustomCsTemplateDefinition());
            definitions.Add(new TDTests.WcfServiceTestsGeneratedCsTableTemplateDefinition());
            definitions.Add(new TDTests.WcfServiceTestsGeneratedCsBusinessTemplateDefinition());

            #endregion

            #region [ Custom Templates Definitions ]

            // Add your custom templates here...

            #endregion

            return definitions;
        }
    }
}

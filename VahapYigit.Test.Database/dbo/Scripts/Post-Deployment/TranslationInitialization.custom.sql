-- -----------------------------------------------
-- This file is part of the LayerCake Generator.

-- Copyright (c) 2012, 2013 LayerCake Generator.
-- http://www.layercake-generator.net
-----------------------------------------------

-- DO NOT ADD THIS FILE TO THE DATABASE PROJECT!

-- NOTE: this script is executed on the target database when generated.
-- NOTE: report to the TranslationInitialization.generated.sql file to get the syntax.

-- EXAMPLE
-- INSERT INTO [Translation] ([Translation_Id], [Translation_enUS], [Translation_frFR], [Translation_esES], [Translation_deDE]) SELECT 'Custom.Exception.AnErrorMessage', 'enUS_Custom.Exception.AnErrorMessage', 'frFR_Custom.Exception.AnErrorMessage', 'esES_Custom.Exception.AnErrorMessage', 'deDE_Custom.Exception.AnErrorMessage' WHERE NOT(EXISTS(SELECT * FROM [Translation] WHERE [Translation_Id] = 'Custom.Exception.AnErrorMessage'));
-- INSERT INTO [Translation] ([Translation_Id], [Translation_enUS], [Translation_frFR], [Translation_esES], [Translation_deDE]) SELECT 'Custom.Exception.ResourceAccessError', 'enUS_Custom.Exception.ResourceAccessError', 'frFR_Custom.Exception.ResourceAccessError', 'esES_Custom.Exception.ResourceAccessError', 'deDE_Custom.Exception.ResourceAccessError' WHERE NOT(EXISTS(SELECT * FROM [Translation] WHERE [Translation_Id] = 'Custom.Exception.ResourceAccessError'));


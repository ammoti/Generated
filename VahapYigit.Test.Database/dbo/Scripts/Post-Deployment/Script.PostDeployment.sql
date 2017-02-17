/*
-----------------------------------------------
This file is part of the LayerCake Generator.

Copyright (c) 2012, 2015 LayerCake Generator.
http://www.layercake-generator.net
-----------------------------------------------
*/

/*
 Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\TranslationInitialization.generated.sql
:r .\TranslationInitialization.custom.sql
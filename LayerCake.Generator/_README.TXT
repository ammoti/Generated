
Issue
-----------------

The following error occurred while compiling the LayerCake Generator solution?

 Assembly 'LayerCake.Generator.X, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null' uses 'Microsoft.SqlServer.Smo, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91'
 which has a higher version than referenced assembly 'Microsoft.SqlServer.Smo, Version=10.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91'

Solution
-----------------

 1. Remove all the Microsoft.SqlServer.* references from each project (path should be C:\Program Files\Microsoft SQL Server\100\SDK\Assemblies\...)
 2. Add them back using the assemblies located in the Libraries\Microsoft SQL Server 11.0\ or Libraries\Microsoft SQL Server 12.0\ directory


Same actions if issues occurred with Microsoft.VisualStudio.TextTemplating.* assemblies.

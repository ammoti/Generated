
In order to remove the following warnings...

Warning	26	SQL71562: Procedure: [dbo].[ProcedureName] has an unresolved reference to object /*[master]*/.[sys].[tables].[name].
Warning	3	SQL71502: Procedure: [dbo].[ProcedureName] has an unresolved reference to object [dbo].[sp_executesql].

You have to (re)add a reference on master database.


  1. Right-click on 'References' (VahapYigit.Test.Database)

  2. Select 'Add Database Reference...'

  3. Select 'System database' ans select 'master'

  4. Press Ok to apply

  5. Rebuild the database project, the warnings are no longer raised

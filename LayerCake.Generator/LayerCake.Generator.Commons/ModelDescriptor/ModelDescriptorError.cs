// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using Microsoft.SqlServer.Management.Smo;

	public class ModelDescriptorError
	{
		#region [ Messages ]

		/// <summary>
		/// "This table name is not allowed because '{0}' keyword is reserved by C#"
		/// </summary>
		public static readonly string TableNameNotAllowedKeywordReserved = "This table name is not allowed because the '{0}' keyword is reserved by C#";

		/// <summary>
		/// "This column name is not allowed because '{0}' keyword is reserved by C#"
		/// </summary>
		public static readonly string ColumnNameNotAllowedKeywordReserved = "This column name is not allowed because the '{0}' keyword is reserved by C#";

		/// <summary>
		/// "This column name is not allowed because 'State' keyword is reserved"
		/// </summary>
		public static readonly string ColumnNameNotAllowedStateKeywordReserved = "This column name is not allowed because the 'State' keyword is reserved";

		/// <summary>
		/// "Primary keys must be prefixed with 'Id'"
		/// </summary>
		public static readonly string PrimaryKeysMustBePrefixedWithId = "Primary keys must be prefixed with 'Id'";

		/// <summary>
		/// "Primary keys must be IDENTITY"
		/// </summary>
		public static readonly string PrimaryKeysMustBeIdentity = "Primary keys must be 'IDENTITY'";

		/// <summary>
		/// "Column names must respect the pattern &lt;TABLE_NAME&gt;_Id&lt;COLUMN_NAME&gt;"
		/// </summary>
		public static readonly string ColumnNamesMustRespectPattern = "Column names must respect the pattern <TABLE_NAME>_<COLUMN_NAME>";

		/// <summary>
		/// "Foreign key names must respect the pattern &lt;TABLE_NAME&gt;_Id&lt;COLUMN_NAME&gt; or &lt;TABLE_NAME&gt;_Id&lt;COLUMN_NAME&gt;&lt;SUFFIX&gt;"
		/// </summary>
		public static readonly string ForeignKeyNamesMustRespectPattern = "Foreign key names must respect the pattern <TABLE_NAME>_Id<COLUMN_NAME> or <TABLE_NAME>_Id<COLUMN_NAME><SUFFIX>";

		/// <summary>
		/// "Report to the documentation to know more about namings"
		/// </summary>
		public static readonly string ReportDocumentationNamings = "Report to the documentation to know more about namings";

		/// <summary>
		/// "This column name is not allowed"
		/// </summary>
		public static readonly string ColumnNameNotAllowed = "This column name is not allowed";

		/// <summary>
		/// "This foreign key must be 'NULL'"
		/// </summary>
		public static readonly string ForeignKeyMustBeNull = "This foreign key must be 'NULL'";

		/// <summary>
		/// "CodeRef columns must be VARCHAR data type"
		/// </summary>
		public static readonly string CodeRefColumnsMustBeVarcharDataType = "CodeRef columns must be 'VARCHAR' data type";

		/// <summary>
		/// "CodeRef columns must be 'NOT NULL'"
		/// </summary>
		public static readonly string CodeRefColumnsMustBeNotNull = "CodeRef columns must be 'NOT NULL'";

		/// <summary>
		/// "CodeRef columns must have an UNIQUE constraint! Execute the following instruction: ALTER TABLE [{1}] ADD CONSTRAINT [UQ_{1}_{2}] UNIQUE ([{0}])"
		/// </summary>
		public static readonly string CodeRefColumnsMustHaveUniqueConstraint = "CodeRef columns must have an UNIQUE constraint! Execute the following instruction: ALTER TABLE [{1}] ADD CONSTRAINT [UQ_{1}_{2}] UNIQUE ([{0}])";

		/// <summary>
		/// "The '{0}' column cannot be defined"
		/// </summary>
		public static readonly string ColumnCannotBeDefined = "The '{0}' column cannot be defined";

		/// <summary>
		/// "Data types mismatch (DataType)"
		/// </summary>
		public static readonly string DataTypesMismatchDataType = "Data types mismatch (DataType)";

		/// <summary>
		/// "Data types mismatch (DataType)"
		/// </summary>
		public static readonly string DataTypesMismatchMaximumLength = "Data types mismatch (MaximumLength)";

		/// <summary>
		/// "Data types mismatch (Nullable)"
		/// </summary>
		public static readonly string DataTypesMismatchNullable = "Data types mismatch (Nullable)";

		/// <summary>
		/// "The '{0}' table must define the '{1}' column"
		/// </summary>
		public static readonly string TableMuseDefineTheColumn = "The '{0}' table must define the '{1}' column";

		#endregion

		#region [ Members ]

		private readonly string _tableName = null;

		private readonly string _columnName = null;

		private readonly string _message = null;

		#endregion

		#region [ Constructors ]

		public ModelDescriptorError(string tableName, string columnName, string message, params object[] args)
		{
			_tableName = tableName;
			_columnName = columnName;

			_message = (args != null && args.Length != 0) ? string.Format(message, args) : message;
		}

		public ModelDescriptorError(Table table, Column column, string message, params object[] args)
			: this(table.Name, column.Name, message, args)
		{
		}

		public ModelDescriptorError(Table table, string columnName, string message, params object[] args)
			: this(table.Name, columnName, message, args)
		{
		}

		#endregion

		#region [ Public Methods ]

		public string GetError()
		{
			return this.ToString();
		}

		#endregion

		#region [ Overrides ]

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(_columnName))
			{
				return string.Format("Table [{0}] - Column [{1}] -> {2}", _tableName, _columnName, _message);
			}

			return string.Format("Table [{0}] -> {1}", _tableName, _message);
		}

		#endregion
	}
}

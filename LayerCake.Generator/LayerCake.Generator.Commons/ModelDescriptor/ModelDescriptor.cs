// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using Microsoft.SqlServer.Management.Smo;

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Xml.Serialization;

	public class ModelDescriptor
	{
		#region [ Members ]

		private readonly SmoContext _smoContext = null;

		private readonly ProcessorContext _context = null;

		#endregion

		#region [ Constructor ]

		public ModelDescriptor(SmoContext smoContext, ProcessorContext context)
		{
			_smoContext = smoContext;
			_context = context;
		}

		#endregion

		#region [ Loaders ]

		public string Load(CancellationToken token)
		{
			StringBuilder sbContent = new StringBuilder(1024 * 32);
			this.Errors = new List<ModelDescriptorError>();

			sbContent.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
			sbContent.AppendFormat("<ModelDescriptorSchema>");
			sbContent.AppendFormat("<Tables>");

			foreach (Table table in _smoContext.GetSmoDatabase().Tables)
			{
				this.CurrentTable = table;

				if (token.IsCancellationRequested)
					return string.Empty;

				if (IsReserved(table.Name))
				{
					this.AddError(table.Name, null, ModelDescriptorError.TableNameNotAllowedKeywordReserved, table.Name);
				}

				if (IsIgnoredTable(table))
					continue;

				#region [ Table ]

				sbContent.AppendFormat("<Table>");

				bool withCodeRef = HasCodeRefColumn(table);

				sbContent.AppendFormat("<Name>{0}</Name>", table.Name);
				sbContent.AppendFormat("<HasIdUserColumn>{0}</HasIdUserColumn>", HasIdUserColumn(table).ToString().ToLowerInvariant());
				sbContent.AppendFormat("<HasCodeRefColumn>{0}</HasCodeRefColumn>", withCodeRef.ToString().ToLowerInvariant());
				sbContent.AppendFormat("<IsOwnership>{0}</IsOwnership>", IsOwnershipTable(table).ToString().ToLowerInvariant());

				sbContent.AppendFormat("<Columns>");

				#endregion

				IList<string> languageColumnNames = new List<string>();

				foreach (Column column in table.Columns)
				{
					this.CurrentColumn = column;

					if (token.IsCancellationRequested)
						return string.Empty;

					if (!CheckColumn(column))
						continue;

					if (!CheckIfPrimaryKeyColumn(column))
						continue;

					if (!CheckIfForeignKeyColumn(column))
						continue;

					if (!CheckIfCodeRefColumn(column))
						continue;

					if (languageColumnNames.Contains(column.Name))
					{
						continue;
					}

					languageColumnNames.Clear();

					LanguageColumnInfo lcInfo = this.GetLanguageColumnInfo(column);
					languageColumnNames = lcInfo.LanguageColumnNames;

					SqlCSharpConvertionTypeInfo typeInfo = new SqlCSharpConvertionTypeInfo(column);

					#region [ Column ]

					sbContent.AppendFormat("<Column>");

					if (lcInfo.IsLanguageColumn)
					{
						string csharpPropertyName = GetCSharpPropertyName(table.Name, lcInfo.ColumnNameWithoutCulture);

						sbContent.AppendFormat("<Name>{0}</Name>", lcInfo.ColumnNameWithoutCulture);
						sbContent.AppendFormat("<IsNullable>{0}</IsNullable>", lcInfo.IsNullable.ToString().ToLowerInvariant());
						sbContent.AppendFormat("<CSharpPropertyName>{0}</CSharpPropertyName>", csharpPropertyName);
					}
					else
					{
						sbContent.AppendFormat("<Name>{0}</Name>", column.Name);
						sbContent.AppendFormat("<IsNullable>{0}</IsNullable>", typeInfo.IsNullable.ToString().ToLowerInvariant());
						sbContent.AppendFormat("<CSharpPropertyName>{0}</CSharpPropertyName>", GetCSharpPropertyName(column));
					}

					sbContent.AppendFormat("<SqlTypeName>{0}</SqlTypeName>", typeInfo.SqlTypeName);
					sbContent.AppendFormat("<SqlTypeNameEx>{0}</SqlTypeNameEx>", typeInfo.SqlTypeNameEx);

					if (typeInfo.CSharpType == typeof(string))
					{
						sbContent.AppendFormat("<MaximumLength>{0}</MaximumLength>", typeInfo.MaximumLength);
					}

					if (typeInfo.CSharpType == typeof(decimal) ||
						typeInfo.CSharpType == typeof(decimal?))
					{
						sbContent.AppendFormat("<NumericPrecision>{0}</NumericPrecision>", typeInfo.NumericPrecision);
						sbContent.AppendFormat("<NumericScale>{0}</NumericScale>", typeInfo.NumericScale);
					}

					if (typeInfo.CSharpType == typeof(DateTime?))
					{
						sbContent.AppendFormat("<NumericScale>{0}</NumericScale>", typeInfo.NumericScale);
					}

					sbContent.AppendFormat("<CSharpTypeName>{0}</CSharpTypeName>", typeInfo.CSharpTypeName);
					sbContent.AppendFormat("<CSharpIsTypeNullable>{0}</CSharpIsTypeNullable>", typeInfo.CSharpIsTypeNullable.ToString().ToLowerInvariant());

					sbContent.AppendFormat("<IsIdColumn>{0}</IsIdColumn>", IsIdColumn(column).ToString().ToLowerInvariant());
					sbContent.AppendFormat("<IsOwnershipColumn>{0}</IsOwnershipColumn>", IsOwnershipColumn(column).ToString().ToLowerInvariant());

					#region [ Description ]

					string description = SmoHelper.GetColumnDescription(column);
					if (description != null)
					{
						sbContent.AppendFormat("<Description><![CDATA[{0}]]></Description>", SmoHelper.GetColumnDescription(column));
					}

					#endregion

					#region [ LanguageColumns ]

					sbContent.AppendFormat("<IsLanguageColumn>{0}</IsLanguageColumn>", lcInfo.IsLanguageColumn.ToString().ToLowerInvariant());

					if (lcInfo.IsLanguageColumn)
					{
						sbContent.AppendFormat("<LanguageColumns>");

						foreach (string languageColumnName in lcInfo.LanguageColumnNames)
						{
							sbContent.AppendFormat("<LanguageColumn>");
							sbContent.AppendFormat("<Name>{0}</Name>", languageColumnName);
							sbContent.AppendFormat("<Culture>{0}</Culture>", languageColumnName.Substring(languageColumnName.LastIndexOf('_') + 1));
							sbContent.AppendFormat("<CSharpPropertyName>{0}</CSharpPropertyName>", GetCSharpPropertyName(table.Name, languageColumnName));
							sbContent.AppendFormat("</LanguageColumn>");
						}

						sbContent.AppendFormat("</LanguageColumns>");
					}

					#endregion

					#region [ ForeignKey ]

					sbContent.AppendFormat("<IsForeignKey>{0}</IsForeignKey>", column.IsForeignKey.ToString().ToLowerInvariant());

					if (column.IsForeignKey)
					{
						sbContent.AppendFormat("<ForeignKey>");

						SmoForeignKeyColumnInfo fkInfo = SmoHelper.GetForeignKeyColumnInfo(column);

						sbContent.AppendFormat("<TableName>{0}</TableName>", fkInfo.ReferencedTable.Name);
						sbContent.AppendFormat("<ColumnName>{0}</ColumnName>", fkInfo.ReferencedColumn.Name);

						sbContent.AppendFormat("<CSharpPropertyName>{0}</CSharpPropertyName>",
							GetCSharpPropertyName(table.Name, column.Name).Substring(2));

						sbContent.AppendFormat("</ForeignKey>");
					}

					#endregion

					#region [ DefaultValues ]

					if (lcInfo.IsLanguageColumn)
					{
						sbContent.AppendFormat("<DefaultValues>");

						foreach (string culture in _context.Culture.SupportedCultures)
						{
							sbContent.AppendFormat("<DefaultValue>");
							sbContent.AppendFormat("<Culture>{0}</Culture>", culture);

							if (lcInfo.DefaultValues.ContainsKey(culture) &&
								lcInfo.DefaultValues[culture] != null)
							{
								sbContent.AppendFormat("<Value><![CDATA[{0}]]></Value>", lcInfo.DefaultValues[culture]);
							}
							else
							{
								sbContent.AppendFormat("<Value><![CDATA[null]]></Value>");
							}

							sbContent.AppendFormat("</DefaultValue>");
						}

						sbContent.AppendFormat("</DefaultValues>");
					}
					else
					{
						string defaultValue;
						bool hasDefaultValue = SmoHelper.ParseColumnDefaultValue(column, out defaultValue);

						if (hasDefaultValue)
						{
							sbContent.AppendFormat("<DefaultValue><![CDATA[{0}]]></DefaultValue>", defaultValue);
						}
					}

					#endregion

					sbContent.AppendFormat("</Column>");

					#endregion
				}

				sbContent.AppendFormat("</Columns>");

				#region [ CodeRefs ]

				if (withCodeRef)
				{
					sbContent.AppendFormat("<CodeRefs>");

					foreach (var codeRef in DatabaseHelper.GetCodeRefs(_smoContext, table.Name))
					{
						sbContent.AppendFormat("<CodeRef><Id>{0}</Id><Value>{1}</Value></CodeRef>", codeRef.Id, codeRef.Value);
					}

					sbContent.AppendFormat("</CodeRefs>");
				}

				#endregion

				#region [ Constraints ]

				bool hasUniqueConstraints = false;
				foreach (Index index in table.Indexes)
				{
					if (index.IndexKeyType == IndexKeyType.DriUniqueKey)
					{
						hasUniqueConstraints = true;
						break;
					}
				}

				if (hasUniqueConstraints)
				{
					sbContent.AppendFormat("<Constraints>");

					foreach (Index index in table.Indexes)
					{
						if (token.IsCancellationRequested)
							return string.Empty;

						if (index.IndexKeyType == IndexKeyType.DriUniqueKey)
						{
							sbContent.AppendFormat("<Unique>");
							sbContent.AppendFormat("<Columns>");

							string idxColumnName = null;
							foreach (IndexedColumn idxColumn in index.IndexedColumns)
							{
								if (idxColumnName == null)
								{
									idxColumnName = idxColumn.Name;
								}
								else
								{
									idxColumnName = string.Format("{0}_{1}", idxColumnName, idxColumn.Name);
								}

								string culture;
								string columnNameWithoutCulture;

								bool isLanguageColumn = this.IsLanguageColumn(idxColumn.Name, out culture, out columnNameWithoutCulture);

								sbContent.AppendFormat("<Column>");
								sbContent.AppendFormat("<Name>{0}</Name>", idxColumn.Name);
								sbContent.AppendFormat("<NameWithoutTableName>{0}</NameWithoutTableName>", WithoutTableName(table.Name, idxColumn.Name));

								if (isLanguageColumn)
								{
									sbContent.AppendFormat("<Culture>{0}</Culture>", culture);
									sbContent.AppendFormat("<NameWithoutCulture>{0}</NameWithoutCulture>", columnNameWithoutCulture);
									sbContent.AppendFormat("<NameWithoutTableNameWithoutCulture>{0}</NameWithoutTableNameWithoutCulture>", WithoutTableName(table.Name, columnNameWithoutCulture));
								}

								sbContent.AppendFormat("</Column>");
							}

							sbContent.AppendFormat("</Columns>");
							sbContent.AppendFormat("</Unique>");
						}
					}

					sbContent.AppendFormat("</Constraints>");
				}

				#endregion

				#region [ Relations ]

				#region [ ZeroToOne ]

				IList<ZeroToOneRelationInfo> zeroToOneRelations = GetZeroToOneRelations(table);
				if (zeroToOneRelations.Count != 0)
				{
					sbContent.AppendFormat("<ZeroToOneRelations>");

					foreach (var relation in zeroToOneRelations)
					{
						try
						{
							string csharpPropertyName = GetCSharpPropertyName(relation.TableName, relation.ColumnName).Substring(2);

							sbContent.AppendFormat("<ZeroToOneRelation>");

							sbContent.AppendFormat("<IsLoop>{0}</IsLoop>", relation.IsLoop.ToString().ToLowerInvariant());

							sbContent.AppendFormat("<TableName>{0}</TableName>", relation.TableName);
							sbContent.AppendFormat("<ColumnName>{0}</ColumnName>", relation.ColumnName);

							sbContent.AppendFormat("<ReferencedTableName>{0}</ReferencedTableName>", relation.ReferencedTableName);
							sbContent.AppendFormat("<ReferencedColumnName>{0}</ReferencedColumnName>", relation.ReferencedColumnName);

							sbContent.AppendFormat("<CSharpPropertyName>{0}</CSharpPropertyName>", csharpPropertyName);

							sbContent.AppendFormat("</ZeroToOneRelation>");
						}
						catch
						{
							this.AddError(table.Name, relation.ColumnName, ModelDescriptorError.ForeignKeyNamesMustRespectPattern);
							this.AddError(table.Name, relation.ColumnName, ModelDescriptorError.ReportDocumentationNamings);
							break;
						}
					}

					sbContent.AppendFormat("</ZeroToOneRelations>");
				}

				#endregion

				#region [ ZeroToMany ]

				IList<ZeroToManyRelationInfo> zeroToManyRelations = GetZeroToManyRelations(table);
				if (zeroToManyRelations.Count != 0)
				{
					sbContent.AppendFormat("<ZeroToManyRelations>");

					foreach (var relation in zeroToManyRelations)
					{
						try
						{
							string csharpPropertyName = GetCSharpPropertyName(relation.ReferencedTableName, relation.ColumnName).Substring(2);

							sbContent.AppendFormat("<ZeroToManyRelation>");

							sbContent.AppendFormat("<IsLoop>{0}</IsLoop>", relation.IsLoop.ToString().ToLowerInvariant());

							sbContent.AppendFormat("<TableName>{0}</TableName>", relation.TableName);
							sbContent.AppendFormat("<ColumnName>{0}</ColumnName>", relation.ColumnName);

							sbContent.AppendFormat("<ReferencedTableName>{0}</ReferencedTableName>", relation.ReferencedTableName);
							sbContent.AppendFormat("<ReferencedColumnName>{0}</ReferencedColumnName>", relation.ReferencedColumnName);

							csharpPropertyName = csharpPropertyName.TrimStart(relation.TableName);
							csharpPropertyName = string.Concat(relation.ReferencedTableName, csharpPropertyName);

							sbContent.AppendFormat("<CSharpPropertyName>{0}</CSharpPropertyName>", csharpPropertyName);

							sbContent.AppendFormat("</ZeroToManyRelation>");
						}
						catch
						{
							this.AddError(relation.ReferencedTableName, relation.ColumnName, ModelDescriptorError.ForeignKeyNamesMustRespectPattern);
							this.AddError(relation.ReferencedTableName, relation.ColumnName, ModelDescriptorError.ReportDocumentationNamings);
							break;
						}
					}

					sbContent.AppendFormat("</ZeroToManyRelations>");
				}

				#endregion

				#endregion

				sbContent.AppendFormat("</Table>");
			}

			sbContent.AppendFormat("</Tables>");

			sbContent.AppendFormat("</ModelDescriptorSchema>");

			string descriptorFile = Path.Combine(Path.GetTempPath(), "LayerCake Generator", _context.ProjectName, string.Format("{0}.xml", _context.ProjectName));
			if (!Directory.Exists(Path.GetDirectoryName(descriptorFile)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(descriptorFile));
			}

			File.WriteAllText(descriptorFile, sbContent.ToString());

			this.Load(descriptorFile);
			return descriptorFile;
		}

		public void Load(string filePath)
		{
			if (!File.Exists(filePath))
			{
				ThrowException.ThrowFileNotFoundException("Cannot find the specified MDF file", filePath);
			}

			StreamReader fileReader = null;

			try
			{
				fileReader = new StreamReader(filePath);
				XmlSerializer serializer = new XmlSerializer(typeof(ModelDescriptorSchema)); // can raise 2 FileNotFoundException -> continue execution

				this.Schema = (ModelDescriptorSchema)serializer.Deserialize(fileReader);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (fileReader != null)
				{
					fileReader.Close();
				}
			}
		}

		#endregion

		#region [ Private methods ]

		private static bool IsIgnoredTable(Table table)
		{
			if (table.IsSystemObject || table.FakeSystemTable)
			{
				return true;
			}

			string ucTableName = table.Name.ToUpperInvariant();

			IList<string> ignoredTables = new List<string>
			{
				// Enter here the tables names to ignore 'UPPERCASE'
			};

			if (ignoredTables.Contains(ucTableName))
			{
				return true;
			}

			if (ucTableName.EndsWith("_LOGS"))
			{
				return true;
			}

			return false;
		}

		public static bool IsReserved(string word)
		{
			if (string.IsNullOrEmpty(word))
			{
				return false;
			}

			// http://msdn.microsoft.com/fr-fr/library/x53a06bb.aspx

			var list = new List<string>
			{
				"abstract", "as", "base", "bool", "break", "byte", "case", "catch",
				"char", "checked", "class", "const", "continue", "decimal", "default", "delegate",
				"do", "double", "else", "enum", "event", "explicit", "extern", "false",
				"finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit",
				"in", "int", "interface", "internal", "is", "lock", "long", "namespace",
				"new", "null", "object", "operator", "out", "override", "params", "private",
				"protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
				"sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true",
				"try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
				"virtual", "void", "volatile", "while",

				"add", "alias", "ascending", "async", "await", "descending", "dynamic", "from",
				"get", "global", "group", "into", "join", "let", "orderby", "partial",
				"remove", "select", "set", "value", "var", "where", "yield"
			};

			return list.Contains(word);
		}

		private bool CheckColumn(Column column)
		{
			Table table = (Table)column.Parent;
			string columnName = WithoutTableName(table.Name, column.Name);

			if (column.IsForeignKey) // important to have the right error message (FK vs PK)
			{
				if (!column.Name.StartsWith(string.Format("{0}_Id", table.Name)))
				{
					SmoForeignKeyColumnInfo fkInfo = SmoHelper.GetForeignKeyColumnInfo(column);

					this.AddError(table.Name, column.Name, ModelDescriptorError.ForeignKeyNamesMustRespectPattern);
					this.AddError(table.Name, column.Name, ModelDescriptorError.ReportDocumentationNamings);
					return false;
				}
			}
			else
			{
				if (!column.Name.StartsWith(string.Format("{0}_", table.Name)))
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.ColumnNamesMustRespectPattern);
					this.AddError(table.Name, column.Name, ModelDescriptorError.ReportDocumentationNamings);
					return false;
				}
			}

			if (IsReserved(columnName))
			{
				string alternativeName = string.Format("{0}_{1}{2}", table.Name, columnName[0].ToString().ToUpperInvariant(), columnName.Substring(1));

				this.AddError(table.Name, column.Name, ModelDescriptorError.ColumnNameNotAllowedKeywordReserved, columnName);
				//this.AddError(table.Name, column.Name, "Try with '{0}'.", alternativeName);
				return false;
			}

			if (column.Name.Equals(string.Format("{0}_State", table.Name)))
			{
				this.AddError(table.Name, column.Name, ModelDescriptorError.ColumnNameNotAllowedStateKeywordReserved);
				return false;
			}

			return true;
		}

		private bool CheckIfPrimaryKeyColumn(Column column)
		{
			if (column.InPrimaryKey)
			{
				Table table = (Table)column.Parent;

				if (!column.Name.Equals(string.Format("{0}_Id", table.Name)))
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.PrimaryKeysMustBePrefixedWithId);
					this.AddError(table.Name, column.Name, ModelDescriptorError.ReportDocumentationNamings);
					return false;
				}

				if (!column.Identity)
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.PrimaryKeysMustBeIdentity);
					return false;
				}
			}

			return true;
		}

		private bool CheckIfForeignKeyColumn(Column column)
		{
			if (column.IsForeignKey)
			{
				Table table = (Table)column.Parent;
				SmoForeignKeyColumnInfo fkInfo = SmoHelper.GetForeignKeyColumnInfo(column);

				if (!column.Name.StartsWith(string.Format("{0}_Id{1}", table.Name, fkInfo.ReferencedTable.Name)))
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.ForeignKeyNamesMustRespectPattern);
					this.AddError(table.Name, column.Name, ModelDescriptorError.ReportDocumentationNamings);
					//this.AddError(table.Name, column.Name, "Try with '{0}_Id{1}' or '{0}_Id{1}Suffix'", table.Name, fkInfo.ReferencedTable.Name);
					return false;
				}

				string columnNameWithoutTableName = WithoutTableName(table.Name, column.Name);
				if (columnNameWithoutTableName.Substring(2) == table.Name)
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.ColumnNameNotAllowed);
					//this.AddError(table.Name, column.Name, "Try with a suffix '{0}Parent'", column.Name);
					return false;
				}

				if (fkInfo.ReferencedTable.Name == table.Name && !column.Nullable)
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.ForeignKeyMustBeNull);
					return false;
				}
			}

			return true;
		}

		private bool CheckIfCodeRefColumn(Column column)
		{
			Table table = (Table)column.Parent;

			if (string.Compare(WithoutTableName(table.Name, column.Name), "CodeRef", true) == 0)
			{
				SqlCSharpConvertionTypeInfo typeInfo = new SqlCSharpConvertionTypeInfo(column);
				if (typeInfo.CSharpType != typeof(string))
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.CodeRefColumnsMustBeVarcharDataType);
					return false;
				}

				if (column.Nullable)
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.CodeRefColumnsMustBeNotNull);
					return false;
				}

				bool bIsUnique = false;
				foreach (Index index in table.Indexes)
				{
					if (index.IndexKeyType == IndexKeyType.DriUniqueKey)
					{
						foreach (IndexedColumn idxColumn in index.IndexedColumns)
						{
							bIsUnique = string.Compare(idxColumn.Name, WithTableName(table, "CodeRef"), true) == 0;
							if (bIsUnique)
							{
								break;
							}
						}
					}

					if (bIsUnique)
					{
						break;
					}
				}

				if (!bIsUnique)
				{
					this.AddError(table.Name, column.Name, ModelDescriptorError.CodeRefColumnsMustHaveUniqueConstraint,
						column.Name, table.Name, WithoutTableName(table.Name, column.Name));

					return false;
				}
			}

			return true;
		}

		private static bool IsIdColumn(Column column)
		{
			Table table = (Table)column.Parent;

			return column.Name.Remove(0, table.Name.Length + 1).Equals("Id");
		}

		private bool HasIdUserColumn(Table table)
		{
			string idUserColumnName = string.Format("{0}_{1}", table.Name, _context.AuthenticationSupport.IdUserColumnName);

			foreach (Column column in table.Columns)
			{
				if (column.Name.Equals(idUserColumnName))
					return true;
			}

			return false;
		}

		private static bool IsOwnershipTable(Table table)
		{
			return
				table.Columns[WithTableName(table, "CreatedOn")] != null &&
				table.Columns[WithTableName(table, "CreatedBy")] != null &&
				table.Columns[WithTableName(table, "ModifiedOn")] != null &&
				table.Columns[WithTableName(table, "ModifiedBy")] != null &&
				table.Columns[WithTableName(table, "Owner")] != null &&
				table.Columns[WithTableName(table, "IsLocked")] != null;
		}

		private static bool IsOwnershipColumn(Column column)
		{
			Table table = (Table)column.Parent;

			if (IsOwnershipTable(table))
			{
				var _list = new List<string>()
				{
					string.Format("{0}_CreatedOn", table.Name),
					string.Format("{0}_CreatedBy", table.Name),
					string.Format("{0}_ModifiedOn", table.Name),
					string.Format("{0}_ModifiedBy", table.Name),
					string.Format("{0}_Owner", table.Name),
					string.Format("{0}_IsLocked", table.Name)
				};

				return _list.Contains(column.Name);
			}

			return false;
		}

		private static string WithTableName(Table table, string columnNameWithoutPrefix)
		{
			return string.Format("{0}_{1}", table.Name, columnNameWithoutPrefix);
		}

		private static string WithoutTableName(string tableName, string columnName)
		{
			if (columnName.StartsWith(string.Format("{0}_", tableName)))
			{
				return columnName.Substring(tableName.Length + 1);
			}

			return columnName; // bad name
		}

		private bool IsLanguageColumn(string columnName, out string culture, out string columnNameWithoutCulture)
		{
			culture = null;
			columnNameWithoutCulture = string.Empty;

			int cultureLength = _context.Culture.SupportedCultures[0].Length;

			if (columnName.Length < cultureLength + 1)
			{
				return false;
			}

			bool isLanguageColumn = IsCultureSupported(
				columnName.Substring(columnName.Length - cultureLength));

			if (isLanguageColumn)
			{
				culture = columnName.Substring(columnName.LastIndexOf('_') + 1);

				columnNameWithoutCulture =
					columnName.Substring(0, columnName.Length - cultureLength - 1);
			}

			return isLanguageColumn;
		}

		private bool IsCultureSupported(string culture)
		{
			if (culture == null)
			{
				return false;
			}

			culture = culture.Replace("-", "");
			return _context.Culture.SupportedCultures.Contains(culture);
		}

		// REWRITTEN...

		private static IList<ZeroToOneRelationInfo> GetZeroToOneRelations(Table table)
		{
			IList<ZeroToOneRelationInfo> relations = new List<ZeroToOneRelationInfo>();

			foreach (Column column in table.Columns)
			{
				if (!column.IsForeignKey)
					continue;

				ZeroToOneRelationInfo relation = new ZeroToOneRelationInfo();
				SmoForeignKeyColumnInfo fkInfo = SmoHelper.GetForeignKeyColumnInfo(column);

				relation.TableName = table.Name;
				relation.ColumnName = column.Name;

				relation.ReferencedTableName = fkInfo.ReferencedTable.Name;
				relation.ReferencedColumnName = fkInfo.ReferencedColumn.Name;

				relations.Add(relation);
			}

			return relations;
		}

		private static IList<ZeroToManyRelationInfo> GetZeroToManyRelations(Table table)
		{
			IList<ZeroToManyRelationInfo> relations = new List<ZeroToManyRelationInfo>();

			Database database = (Database)table.Parent;

			foreach (Table refTable in database.Tables)
			{
				/*if (refTable == table) // Entity.generated.cs / IsLoop = true / commented to support hierarchied-entities
					continue;*/

				foreach (Column refColumn in refTable.Columns)
				{
					if (!refColumn.IsForeignKey)
						continue;

					SmoForeignKeyColumnInfo fkInfo = SmoHelper.GetForeignKeyColumnInfo(refColumn);

					if (fkInfo.ReferencedTable.Name != table.Name)
						continue;

					ZeroToManyRelationInfo relation = new ZeroToManyRelationInfo();

					relation.TableName = table.Name;
					relation.ColumnName = refColumn.Name;

					relation.ReferencedTableName = refTable.Name;
					relation.ReferencedColumnName = fkInfo.ReferencedColumn.Name;

					relations.Add(relation);
				}
			}

			return relations;
		}

		private static bool HasCodeRefColumn(Table table)
		{
			foreach (Column column in table.Columns)
			{
				if (column.Name == string.Format("{0}_CodeRef", table.Name))
					return true;
			}

			return false;
		}

		private static string GetCSharpPropertyName(Column column)
		{
			Table table = (Table)column.Parent;
			return GetCSharpPropertyName(table.Name, column.Name);
		}

		private static string GetCSharpPropertyName(string tableName, string columnName)
		{
			return columnName.Substring(tableName.Length + 1);
		}

		private LanguageColumnInfo GetLanguageColumnInfo(Column column)
		{
			Table table = (Table)column.Parent;
			LanguageColumnInfo info = new LanguageColumnInfo();

			int cultureLength = _context.Culture.SupportedCultures[0].Length;
			if (column.Name.Length < cultureLength + 1)
			{
				return info;
			}

			SqlCSharpConvertionTypeInfo typeInfo = new SqlCSharpConvertionTypeInfo(column);
			if (typeInfo.CSharpType != typeof(string))
			{
				return info;
			}

			bool isLanguageColumn = this.IsCultureSupported(column.Name.Substring(column.Name.Length - cultureLength));
			if (!isLanguageColumn)
			{
				return info;
			}

			info.IsLanguageColumn = true;

			info.ColumnNameWithoutCulture =
				column.Name.Substring(0, column.Name.Length - cultureLength - 1);

			foreach (Column c in table.Columns)
			{
				if (string.Compare(c.Name, info.ColumnNameWithoutCulture, true) == 0)
				{
					this.AddError(table.Name, null, ModelDescriptorError.ColumnCannotBeDefined, c.Name);
				}
			}

			foreach (string culture in _context.Culture.SupportedCultures)
			{
				bool bColumnFound = false;
				string cName = string.Format("{0}_{1}", info.ColumnNameWithoutCulture, culture);

				foreach (Column tColumn in table.Columns)
				{
					if (tColumn.Name == cName)
					{
						bColumnFound = true;
						info.LanguageColumnNames.Add(tColumn.Name);

						string defaultValue = null;
						bool bHasDefaultValue = SmoHelper.ParseColumnDefaultValue(tColumn, out defaultValue);

						if (bHasDefaultValue)
						{
							info.DefaultValues.Add(culture, defaultValue);
						}

						if (tColumn.DataType.Name != column.DataType.Name)
						{
							this.AddError(table.Name, column.Name, ModelDescriptorError.DataTypesMismatchDataType, table.Name, column.Name);
						}

						if (tColumn.DataType.MaximumLength != column.DataType.MaximumLength)
						{
							this.AddError(table.Name, column.Name, ModelDescriptorError.DataTypesMismatchMaximumLength, table.Name, column.Name);
						}

						if (tColumn.Nullable != column.Nullable)
						{
							this.AddError(table.Name, column.Name, ModelDescriptorError.DataTypesMismatchNullable, table.Name, column.Name);
						}

						info.IsNullable = column.Nullable;
					}
				}

				if (!bColumnFound)
				{
					this.AddError(table.Name, null, ModelDescriptorError.TableMuseDefineTheColumn, table.Name, cName);
				}
			}

			return info;
		}

		#region [ Error Methods ]

		private void AddError(string tableName, string columnName, string error, params object[] args)
		{
			this.Errors.Add(new ModelDescriptorError(tableName, columnName, error, args));
		}

		#endregion

		#endregion

		#region [ Properties ]

		public IList<ModelDescriptorError> Errors
		{
			get;
			private set;
		}

		public ModelDescriptorSchema Schema
		{
			get;
			private set;
		}

		#endregion

		#region [ Exception Trackers ]

		public Table CurrentTable { get; private set; }

		public Column CurrentColumn { get; private set; }

		#endregion
	}
}

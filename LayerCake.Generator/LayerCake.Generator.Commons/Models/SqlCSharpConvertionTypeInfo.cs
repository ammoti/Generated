// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;

	using Microsoft.SqlServer.Management.Smo;

	public class SqlCSharpConvertionTypeInfo
	{
		#region [ Constructor ]

		public SqlCSharpConvertionTypeInfo(Column column)
		{
			this.Convert(column);
		}

		#endregion

		#region [ Methods ]

		private void Convert(Column column)
		{
			// SQL Server Data Type Mappings
			// http://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx

			this.SqlType = column.DataType.SqlDataType;
			this.SqlTypeName = column.DataType.Name.ToUpperInvariant();
			this.SqlTypeNameEx = this.SqlTypeName;

			#region [ SQL -> C# Convert ]

			switch (this.SqlType)
			{
				case SqlDataType.VarChar:
				case SqlDataType.VarCharMax:
				case SqlDataType.NVarChar:
				case SqlDataType.NVarCharMax:
				case SqlDataType.Text:
				case SqlDataType.NText:
				case SqlDataType.Char:
				case SqlDataType.NChar:
					this.CSharpTypeName = "string";
					this.CSharpType = typeof(string);
					break;

				case SqlDataType.TinyInt:
					if (column.Nullable)
					{
						this.CSharpTypeName = "byte?";
						this.CSharpType = typeof(byte?);
					}
					else
					{
						this.CSharpTypeName = "byte";
						this.CSharpType = typeof(byte);
					}
					break;

				case SqlDataType.SmallInt:
					if (column.Nullable)
					{
						this.CSharpTypeName = "short?";
						this.CSharpType = typeof(short?);
					}
					else
					{
						this.CSharpTypeName = "short";
						this.CSharpType = typeof(short);
					}
					break;

				case SqlDataType.Int:
					if (column.Nullable)
					{
						this.CSharpTypeName = "int?";
						this.CSharpType = typeof(int?);
					}
					else
					{
						this.CSharpTypeName = "int";
						this.CSharpType = typeof(int);
					}
					break;

				case SqlDataType.BigInt:
					if (column.Nullable)
					{
						this.CSharpTypeName = "long?";
						this.CSharpType = typeof(long?);
					}
					else
					{
						this.CSharpTypeName = "long";
						this.CSharpType = typeof(long);
					}
					break;

				case SqlDataType.Float:
					if (column.Nullable)
					{
						this.CSharpTypeName = "double?";
						this.CSharpType = typeof(double?);
					}
					else
					{
						this.CSharpTypeName = "double";
						this.CSharpType = typeof(double);
					}
					break;

				case SqlDataType.Decimal:
				case SqlDataType.Numeric:
				case SqlDataType.Money:
				case SqlDataType.SmallMoney:
					if (column.Nullable)
					{
						this.CSharpTypeName = "decimal?";
						this.CSharpType = typeof(decimal?);
					}
					else
					{
						this.CSharpTypeName = "decimal";
						this.CSharpType = typeof(decimal);
					}
					break;

				case SqlDataType.Real:
					if (column.Nullable)
					{
						this.CSharpTypeName = "Single?";
						this.CSharpType = typeof(Single?);
					}
					else
					{
						this.CSharpTypeName = "Single";
						this.CSharpType = typeof(Single);
					}
					break;

				case SqlDataType.Date:
				case SqlDataType.SmallDateTime:
				case SqlDataType.DateTime:
				case SqlDataType.DateTime2:
					if (column.Nullable)
					{
						this.CSharpTypeName = "DateTime?";
						this.CSharpType = typeof(DateTime?);
					}
					else
					{
						this.CSharpTypeName = "DateTime";
						this.CSharpType = typeof(DateTime);
					}
					break;

				case SqlDataType.Time:
					if (column.Nullable)
					{
						this.CSharpTypeName = "TimeSpan?";
						this.CSharpType = typeof(TimeSpan?);
					}
					else
					{
						this.CSharpTypeName = "TimeSpan";
						this.CSharpType = typeof(TimeSpan);
					}
					break;

				case SqlDataType.DateTimeOffset:
					if (column.Nullable)
					{
						this.CSharpTypeName = "DateTimeOffset?";
						this.CSharpType = typeof(DateTimeOffset?);
					}
					else
					{
						this.CSharpTypeName = "DateTimeOffset";
						this.CSharpType = typeof(DateTimeOffset);
					}
					break;

				case SqlDataType.Bit:
					if (column.Nullable)
					{
						this.CSharpTypeName = "bool?";
						this.CSharpType = typeof(bool?);
					}
					else
					{
						this.CSharpTypeName = "bool";
						this.CSharpType = typeof(bool);
					}
					break;

				case SqlDataType.UniqueIdentifier:
					if (column.Nullable)
					{
						this.CSharpTypeName = "Guid?";
						this.CSharpType = typeof(Guid?);
					}
					else
					{
						this.CSharpTypeName = "Guid";
						this.CSharpType = typeof(Guid);
					}
					break;

				default:
					this.CSharpType = null;
					this.CSharpTypeName = string.Format("#? (SqlDataType = {0}) ?#", column.DataType.SqlDataType);
					break;
			}

			#endregion

			this.IsNullable = column.Nullable;
			this.CSharpIsTypeNullable = this.CSharpTypeName == "string" || this.CSharpTypeName.EndsWith("?");

			#region [ SqlTypeNameEx ]

			if (this.CSharpType == typeof(string))
			{
				this.MaximumLength = column.DataType.MaximumLength;

				this.SqlTypeNameEx = string.Format("{0}({1})",
					this.SqlTypeName,
					this.MaximumLength < 0 ? "MAX" : this.MaximumLength.ToString());
			}
			else if (this.CSharpType == typeof(decimal) || this.CSharpType == typeof(decimal?))
			{
				this.NumericPrecision = column.DataType.NumericPrecision;
				this.NumericScale = column.DataType.NumericScale;

				this.SqlTypeNameEx = string.Format("DECIMAL({0}, {1})",
					this.NumericPrecision,
					this.NumericScale);
			}
			else if (this.SqlType == SqlDataType.DateTime2)
			{
				this.NumericScale = column.DataType.NumericScale;

				this.SqlTypeNameEx = string.Format("DATETIME2({0})",
					column.DataType.NumericScale);
			}

			#endregion
		}

		#endregion

		#region [ Properties ]

		public SqlDataType SqlType { get; set; }

		public string SqlTypeName { get; set; }

		public string SqlTypeNameEx { get; set; }

		public Type CSharpType { get; set; }

		public string CSharpTypeName { get; set; }

		public bool IsNullable { get; set; }

		public bool CSharpIsTypeNullable { get; set; }

		public int MaximumLength { get; set; }

		public int NumericPrecision { get; set; }

		public int NumericScale { get; set; }

		#endregion
	}
}

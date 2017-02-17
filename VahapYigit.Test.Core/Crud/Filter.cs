// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Filter class.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.Core.Filter", IsReference = true)]
    public class Filter
    {
        private static readonly int SQL_DATETIME_CODE = 121;
        private static readonly string SQL_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        private static readonly string SQL_COLLATION = "French_CI_AI";

        /// <summary>
        /// Default constructor.
        /// </summary>
        private Filter()
        {
            this.AccentSensitivity = AccentSensitivity.Without;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="orGroup">
        /// Or group (filters with the same group value will be linked with AND operand. OR operand is used between filters having different group value).
        /// </param>
        /// 
        /// <param name="columnName">
        /// Column name.
        /// </param>
        /// 
        /// <param name="filterOperator">
        /// Filter operator.
        /// </param>
        /// 
        /// <param name="accentSensitivity">
        /// Accent sensitivity enumeration.
        /// </param>
        public Filter(int orGroup, string columnName, FilterOperator filterOperator, AccentSensitivity accentSensitivity = AccentSensitivity.Without)
            : this()
        {
            this.OrGroup = orGroup;
            this.ColumnName = columnName;
            this.Operator = filterOperator;
            this.AccentSensitivity = accentSensitivity;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="columnName">
        /// Column name.
        /// </param>
        /// 
        /// <param name="filterOperator">
        /// Filter operator.
        /// </param>
        /// 
        /// <param name="accentSensitivity">
        /// Accent sensitivity enumeration.
        /// </param>
        public Filter(string columnName, FilterOperator filterOperator, AccentSensitivity accentSensitivity = AccentSensitivity.Without)
            : this(0, columnName, filterOperator, accentSensitivity)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="orGroup">
        /// Or group (filters with the same group value will be linked with AND operand. OR operand is used between filters having different group value).
        /// </param>
        /// 
        /// <param name="columnName">
        /// Column name.
        /// </param>
        /// 
        /// <param name="filterOperator">
        /// Filter operator.
        /// </param>
        /// 
        /// <param name="value">
        /// Value to add.
        /// </param>
        /// 
        /// <param name="accentSensitivity">
        /// Accent sensitivity enumeration.
        /// </param>
        public Filter(int orGroup, string columnName, FilterOperator filterOperator, object value, AccentSensitivity accentSensitivity = AccentSensitivity.Without)
            : this(orGroup, columnName, filterOperator, accentSensitivity)
        {
            if (filterOperator == FilterOperator.Between || filterOperator == FilterOperator.In || filterOperator == FilterOperator.NotIn)
            {
                this.Values = value as IEnumerable;
            }
            else
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="columnName">
        /// Column name.
        /// </param>
        /// 
        /// <param name="filterOperator">
        /// Filter operator.
        /// </param>
        /// 
        /// <param name="value">
        /// Value to add.
        /// </param>
        /// 
        /// <param name="accentSensitivity">
        /// Accent sensitivity enumeration.
        /// </param>
        public Filter(string columnName, FilterOperator filterOperator, object value, AccentSensitivity accentSensitivity = AccentSensitivity.Without)
            : this(0, columnName, filterOperator, value, accentSensitivity)
        {
        }

        /// <summary>
        /// Gets the inner values into a string representation.
        /// </summary>
        /// 
        /// <returns>
        /// The string representation.
        /// </returns>
        private string GetSqlStringValues()
        {
            StringBuilder sbString = new StringBuilder(16);

            int p = 0;
            IEnumerator iterator = this.Values.GetEnumerator();

            while (iterator.MoveNext())
            {
                if (p != 0)
                {
                    sbString.Append(", ");
                }

                p++;

                if (iterator.Current is string)
                {
                    sbString.AppendFormat("N'{0}'", iterator.Current);
                }
                else if (iterator.Current is DateTime)
                {
                    sbString.AppendFormat("N'{0}'", ((DateTime)iterator.Current).ToString(SQL_DATETIME_FORMAT));
                }
                else
                {
                    sbString.AppendFormat("{0}", iterator.Current);
                }
            }

            return sbString.ToString();
        }

        /// <summary>
        /// Gets the formatted filter.
        /// </summary>
        /// 
        /// <returns>
        /// The formatted filter.
        /// </returns>
        public override string ToString()
        {
            if (this.Operator == FilterOperator.Equals)
            {
                #region [ FilterOperator.Equals ]

                if (this.Value is string)
                {
                    if (this.AccentSensitivity == AccentSensitivity.Without)
                    {
                        return string.Format("[{0}] COLLATE {1} = N'{2}' COLLATE {1}", this.ColumnName, SQL_COLLATION, this.Value);
                    }
                    else
                    {
                        return string.Format("[{0}] = N'{1}'", this.ColumnName, this.Value);
                    }
                }

                if (this.Value is bool)
                {
                    return string.Format("[{0}] = {1}", this.ColumnName, ((bool)this.Value == true) ? 1 : 0);
                }

                if (this.Value is DateTime)
                {
                    return string.Format("[{0}] = CONVERT(DATETIME, '{1}', {2})", this.ColumnName, ((DateTime)this.Value).ToString(SQL_DATETIME_FORMAT), SQL_DATETIME_CODE);
                }

                if (this.Value is decimal || this.Value is double || this.Value is float)
                {
                    return string.Format("[{0}] = {1}", this.ColumnName, this.Value.ToString().Replace(",", "."));
                }

                if (this.Value is Guid)
                {
                    return string.Format("[{0}] = N'{1}'", this.ColumnName, this.Value);
                }

                if (this.Value == null)
                {
                    return string.Format("[{0}] IS NULL", this.ColumnName);
                }

                return string.Format("[{0}] = {1}", this.ColumnName, this.Value);

                #endregion
            }

            if (this.Operator == FilterOperator.Different)
            {
                #region [ FilterOperator.Different ]

                if (this.Value is string)
                {
                    if (this.AccentSensitivity == AccentSensitivity.Without)
                    {
                        return string.Format("[{0}] COLLATE {1} != N'{2}' COLLATE {1}", this.ColumnName, SQL_COLLATION, this.Value);
                    }
                    else
                    {
                        return string.Format("[{0}] != N'{1}'", this.ColumnName, this.Value);
                    }
                }

                if (this.Value is bool)
                {
                    return string.Format("[{0}] != {1}", this.ColumnName, ((bool)this.Value == true) ? 1 : 0);
                }

                if (this.Value is DateTime)
                {
                    return string.Format("[{0}] != CONVERT(DATETIME, '{1}', {2})", this.ColumnName, ((DateTime)this.Value).ToString(SQL_DATETIME_FORMAT), SQL_DATETIME_CODE);
                }

                if (this.Value == null)
                {
                    return string.Format("[{0}] IS NOT NULL", this.ColumnName);
                }

                return string.Format("[{0}] != {1}", this.ColumnName, this.Value);

                #endregion
            }

            if (this.Operator == FilterOperator.Greater)
            {
                #region [ FilterOperator.Greater ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.Value is string)
                {
                    return string.Format("[{0}] > '{1}'", this.ColumnName, this.Value);
                }

                if (this.Value is DateTime)
                {
                    return string.Format("[{0}] > CONVERT(DATETIME, '{1}', {2})", this.ColumnName, ((DateTime)this.Value).ToString(SQL_DATETIME_FORMAT), SQL_DATETIME_CODE);
                }

                return string.Format("[{0}] > {1}", this.ColumnName, this.Value);

                #endregion
            }

            if (this.Operator == FilterOperator.GreaterOrEquals)
            {
                #region [ FilterOperator.GreaterOrEquals ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.Value is string)
                {
                    return string.Format("[{0}] >= '{1}'", this.ColumnName, this.Value);
                }

                if (this.Value is DateTime)
                {
                    return string.Format("[{0}] >= CONVERT(DATETIME, '{1}', {2})", this.ColumnName, ((DateTime)this.Value).ToString(SQL_DATETIME_FORMAT), SQL_DATETIME_CODE);
                }

                return string.Format("[{0}] >= {1}", this.ColumnName, this.Value);

                #endregion
            }

            if (this.Operator == FilterOperator.Less)
            {
                #region [ FilterOperator.Less ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.Value is string)
                {
                    return string.Format("[{0}] < '{1}'", this.ColumnName, this.Value);
                }

                if (this.Value is DateTime)
                {
                    return string.Format("[{0}] < CONVERT(DATETIME, '{1}', {2})", this.ColumnName, ((DateTime)this.Value).ToString(SQL_DATETIME_FORMAT), SQL_DATETIME_CODE);
                }

                return string.Format("[{0}] < {1}", this.ColumnName, this.Value);

                #endregion
            }

            if (this.Operator == FilterOperator.LessOrEquals)
            {
                #region [ FilterOperator.LessOrEquals ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.Value is string)
                {
                    return string.Format("[{0}] <= '{1}'", this.ColumnName, this.Value);
                }

                if (this.Value is DateTime)
                {
                    return string.Format("[{0}] <= CONVERT(DATETIME, '{1}', {2})", this.ColumnName, ((DateTime)this.Value).ToString(SQL_DATETIME_FORMAT), SQL_DATETIME_CODE);
                }

                return string.Format("[{0}] <= {1}", this.ColumnName, this.Value);

                #endregion
            }

            if (this.Operator == FilterOperator.Between)
            {
                #region [ FilterOperator.Between ]

                if (this.Values.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                object value1 = null;
                object value2 = null;

                int p = 1;
                foreach (object value in this.Values)
                {
                    if (p == 1) value1 = value;
                    if (p == 2) value2 = value;
                    p++;
                }

                if (value1 == null || value2 == null)
                {
                    return string.Empty;
                }

                if (value1 is string)
                {
                    return string.Format("[{0}] BETWEEN '{1}' AND '{2}'", this.ColumnName, value1, value2);
                }

                if (value1 is DateTime)
                {
                    return string.Format("[{0}] BETWEEN CONVERT(DATETIME, '{1}', {3}) AND CONVERT(DATETIME, '{2}', {3})",
                        this.ColumnName,
                        ((DateTime)value1).ToString(SQL_DATETIME_FORMAT),
                        ((DateTime)value2).ToString(SQL_DATETIME_FORMAT),
                        SQL_DATETIME_CODE);
                }

                return string.Format("[{0}] BETWEEN {1} AND {2}", this.ColumnName, value1, value2);

                #endregion
            }

            if (this.Operator == FilterOperator.Like)
            {
                #region [ FilterOperator.Contains ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.AccentSensitivity == AccentSensitivity.Without)
                {
                    return string.Format("[{0}] COLLATE {1} LIKE N'%{2}%' COLLATE {1}", this.ColumnName, SQL_COLLATION, this.Value);
                }
                else
                {
                    return string.Format("[{0}] LIKE N'%{1}%'", this.ColumnName, this.Value);
                }

                #endregion
            }

            if (this.Operator == FilterOperator.NotLike)
            {
                #region [ FilterOperator.NotLike ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.AccentSensitivity == AccentSensitivity.Without)
                {
                    return string.Format("[{0}] COLLATE {1} NOT LIKE N'%{2}%' COLLATE {1}", this.ColumnName, SQL_COLLATION, this.Value);
                }
                else
                {
                    return string.Format("[{0}] NOT LIKE N'%{1}%'", this.ColumnName, this.Value);
                }

                #endregion
            }

            if (this.Operator == FilterOperator.In)
            {
                #region [ FilterOperator.In ]

                if (this.Values.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                return string.Format("[{0}] IN ({1})", this.ColumnName, this.GetSqlStringValues());

                #endregion
            }

            if (this.Operator == FilterOperator.NotIn)
            {
                #region [ FilterOperator.NotIn ]

                if (this.Values.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                return string.Format("[{0}] NOT IN ({1})", this.ColumnName, this.GetSqlStringValues());

                #endregion
            }

            if (this.Operator == FilterOperator.StartsWith)
            {
                #region [ FilterOperator.StartsWith ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.AccentSensitivity == AccentSensitivity.Without)
                {
                    return string.Format("[{0}] COLLATE {1} LIKE N'{2}%' COLLATE {1}", this.ColumnName, SQL_COLLATION, this.Value);
                }
                else
                {
                    return string.Format("[{0}] LIKE N'{1}%'", this.ColumnName, this.Value);
                }

                #endregion
            }

            if (this.Operator == FilterOperator.EndsWith)
            {
                #region [ FilterOperator.EndsWith ]

                if (this.Value == null)
                {
                    return string.Empty;
                }

                if (this.AccentSensitivity == AccentSensitivity.Without)
                {
                    return string.Format("[{0}] COLLATE {1} LIKE N'%{2}' COLLATE {1}", this.ColumnName, SQL_COLLATION, this.Value);
                }
                else
                {
                    return string.Format("[{0}] LIKE N'%{1}'", this.ColumnName, this.Value);
                }

                #endregion
            }

            if (this.Operator == FilterOperator.IsNull)
            {
                #region [ FilterOperator.IsNull ]

                return string.Format("[{0}] IS NULL", this.ColumnName);

                #endregion
            }

            if (this.Operator == FilterOperator.IsNotNull)
            {
                #region [ FilterOperator.IsNotNull ]

                return string.Format("[{0}] IS NOT NULL", this.ColumnName);

                #endregion
            }

            if (this.Operator == FilterOperator.IsNullOrEmpty)
            {
                #region [ FilterOperator.IsNullOrEmpty ]

                return string.Format("LEN([{0}]) = 0", this.ColumnName);

                #endregion
            }

            if (this.Operator == FilterOperator.Contains)
            {
                #region [ FilterOperator.Contains ]

                if (this.Value is string)
                {
                    return string.Format("CONTAINS({0}, '{1}')",
                        this.ColumnName,
                        SqlServerFullTextSearchHelper.CreateFullTextSearchExpression(this.Value.ToString()));
                }

                return string.Empty;

                #endregion
            }

            return string.Empty;
        }

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the group (filters with the same group ID will be linked with AND operand. OR operand is used between filters having different group ID).
        /// </summary>
        [DataMember]
        public int OrGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        [DataMember]
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        [DataMember]
        public FilterOperator Operator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        [DataMember]
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        [DataMember]
        public IEnumerable Values
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        [DataMember]
        public AccentSensitivity AccentSensitivity
        {
            get;
            set;
        }

        #endregion
    }
}
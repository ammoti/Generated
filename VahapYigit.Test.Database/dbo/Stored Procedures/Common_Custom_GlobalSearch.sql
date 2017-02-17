CREATE PROCEDURE [dbo].[Common_Custom_GlobalSearch] -- Based on http://www.media-division.com/searching_in_all_tables_and_columns_of_a/
(
	@Keyword VARCHAR(256),
	@RestrictionTableNames VARCHAR(1024),

	@CtxPagingCurrentPage INT = 1,
	@CtxPagingRecordsPerPage INT = 50,

	@CtxUser BIGINT = NULL,						-- not used here
	@CtxCulture VARCHAR(2) = N'EN',				-- not used here
	@CtxWithContextSecurity BIT =  N'True'		-- not used here
)
AS
BEGIN

	SET NOCOUNT ON

	CREATE TABLE #T_ExclusionTables (TableName VARCHAR(128))

	INSERT INTO #T_ExclusionTables
	VALUES ('AppSettings'),
	       ('ExecutionTrace'),
		   ('ProcessErrorLog'),
		   ('ProcessLog'),
		   ('Role'),
		   ('Translation'),
		   ('User'),
		   ('UserRole'),
		   ('sysdiagrams')

	CREATE TABLE #T_Results (TableName VARCHAR(128), ColumnName VARCHAR(512), Id BIGINT, Value VARCHAR(MAX))
 
	DECLARE
		@TableName VARCHAR(128) = '',
		@ColumnName VARCHAR(512),
		@TableNameWithQuoteName VARCHAR(128) = '',
		@ColumnNameWithQuoteName VARCHAR(512)

	SET @Keyword = QUOTENAME('%' + @Keyword + '%', '''')

	WHILE @TableName IS NOT NULL
	BEGIN

		SET @ColumnName = ''
		SET @ColumnNameWithQuoteName = ''

		SELECT
			@TableName =  MIN(TABLE_NAME),
			@TableNameWithQuoteName = MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME))
		FROM
			INFORMATION_SCHEMA.TABLES WITH(NOLOCK)
		WHERE
			TABLE_TYPE = 'BASE TABLE'
		AND TABLE_NAME NOT IN (SELECT TableName FROM #T_ExclusionTables)
		AND (@RestrictionTableNames IS NULL OR TABLE_NAME IN (SELECT LTRIM(RTRIM(Value)) FROM Splitter(@RestrictionTableNames, ',')))
		AND QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableNameWithQuoteName
		AND OBJECTPROPERTY(OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), 'IsMSShipped') = 0
 
		WHILE @TableName IS NOT NULL AND @ColumnName IS NOT NULL
		BEGIN

			SELECT
				@ColumnName = MIN(COLUMN_NAME),
				@ColumnNameWithQuoteName = MIN(QUOTENAME(COLUMN_NAME))
			FROM
				INFORMATION_SCHEMA.COLUMNS WITH(NOLOCK)
			WHERE
				TABLE_SCHEMA = PARSENAME(@TableNameWithQuoteName, 2)
			AND TABLE_NAME = PARSENAME(@TableNameWithQuoteName, 1)
			AND DATA_TYPE IN ('CHAR', 'VARCHAR', 'NCHAR', 'NVARCHAR')
			AND QUOTENAME(COLUMN_NAME) > @ColumnNameWithQuoteName
 
			IF @ColumnName IS NOT NULL
			BEGIN

				INSERT INTO #T_Results
				EXEC (N'SELECT ''' + @TableName + N''', ''' + @ColumnName + N''', ' + @TableName + N'_Id, ' + 'LEFT(' + @ColumnNameWithQuoteName + N', 8000) FROM ' + @TableNameWithQuoteName + N' WITH(NOLOCK)' + N' WHERE ' + @ColumnNameWithQuoteName + N' LIKE ' + @Keyword)

			END
		END

	END

	IF @CtxPagingCurrentPage < 1 BEGIN SET @CtxPagingCurrentPage = 1 END;
	IF @CtxPagingRecordsPerPage < 1 BEGIN SET @CtxPagingRecordsPerPage = 50 END;

	DECLARE @V_LASTROW INT = @CtxPagingCurrentPage * @CtxPagingRecordsPerPage;
	DECLARE @V_FIRSTROW INT = @V_LASTROW - @CtxPagingRecordsPerPage + 1;

	;
	WITH PagedRows
	AS
	(
		SELECT
			ROW_NUMBER() OVER(ORDER BY TableName, ColumnName) AS RowId,
			*
		FROM
			#T_Results
	)
	SELECT
		*
	FROM
		PagedRows WITH(NOLOCK)
	WHERE
		RowId BETWEEN @V_FIRSTROW AND @V_LASTROW

	SELECT COUNT(0) AS TotalRecords FROM #T_Results

END
CREATE PROCEDURE [dbo].[Common_Custom_GetTableSizes] -- Based on http://stackoverflow.com/a/21472000
(
	@CtxUser BIGINT = NULL,						-- not used here
	@CtxCulture VARCHAR(2) = N'EN',				-- not used here
	@CtxWithContextSecurity BIT = N'True'		-- not used here
)
AS
BEGIN
 
	SET NOCOUNT ON
 
	;
	WITH CTE AS
	(
		SELECT
			T.name,
			P.rows,
			SUM (S.used_page_count) as used_pages_count,
			SUM (CASE
					WHEN (I.index_id < 2) THEN (in_row_data_page_count + lob_used_page_count + row_overflow_used_page_count)
					ELSE lob_used_page_count + row_overflow_used_page_count
				 END) AS Pages
		FROM
			[sys].[dm_db_partition_stats] AS S
			JOIN .[sys].[tables] AS T ON S.object_id = T.object_id
			JOIN [sys].[indexes] AS I ON I.object_id = T.object_id AND S.index_id = I.index_id
			JOIN [sys].[partitions] AS P ON P.object_id = I.object_id AND I.index_id = P.index_id
		GROUP BY
			T.name, P.rows
	)
	SELECT
		name,
		rows,
		TableSizeInMB,
		IndexSizeInMB
	FROM
	(
		SELECT
			CTE.name AS Name,
			CTE.rows AS [Rows],
			CAST((CTE.Pages * 8.) / 1024 AS DECIMAL(10,3)) AS TableSizeInMB,
			CAST(((CASE WHEN CTE.used_pages_count > CTE.Pages
					THEN CTE.used_pages_count - CTE.Pages
					ELSE 0
				   END) * 8. / 1024) AS DECIMAL(10,3)) AS IndexSizeInMB
		FROM
			CTE
	) T
	WHERE
		Name != '__RefactorLog'
	ORDER BY
		TableSizeInMB + IndexSizeInMB DESC
 
END
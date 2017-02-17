CREATE FUNCTION [dbo].[CreateFullTextSearchExpression]
(
	@SearchText VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @v_Output VARCHAR(MAX) = RTRIM(LTRIM(@SearchText))
	
	SET @v_Output = REPLACE(@v_Output,'''', ' ')
	SET @v_Output = REPLACE(@v_Output, '"', '')
	SET @v_Output = REPLACE(@v_Output, '&', '')
	SET @v_Output = REPLACE(@v_Output, '-', '')
	
	DECLARE @t_Terms AS TABLE([RowId] INT, [Value] VARCHAR(128))
	
	INSERT INTO @t_Terms
	SELECT [RowId], [Value] FROM dbo.Splitter(@v_Output, ' ') WHERE RTRIM(LTRIM(Value)) != ''
	
	DECLARE @v_Count INT = (SELECT MAX(RowId) FROM @t_Terms)
	DECLARE @v_FormattedText VARCHAR(MAX) = ''
	DECLARE @v_Term VARCHAR(128)
	DECLARE @v_Position INT = 0
	
	WHILE @v_Position <= @v_Count 
	BEGIN
		SET @v_Term = NULL
		SET @v_Term = (SELECT Value FROM @t_Terms WHERE RowId = @v_Position + 1)
		
		IF @v_Term IS NOT NULL
		BEGIN
			IF LEN(@v_FormattedText) != 0
			BEGIN
				SELECT @v_FormattedText += ' '
			END
			SELECT @v_FormattedText += @v_Term
		END
		SELECT @v_Position += 1
	END
      
	SELECT @v_Output = '"' + REPLACE(@v_FormattedText, ' ', '*" AND "') + '*"'
	RETURN @v_Output
END
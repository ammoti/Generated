CREATE FUNCTION [dbo].[Splitter]
(
	@String		VARCHAR(MAX),
	@Delimiter	VARCHAR(2)
)
RETURNS @ValuableTable TABLE ([RowId] INT IDENTITY(1, 1), [Value] VARCHAR(MAX))
BEGIN
	DECLARE @v_NextString NVARCHAR(MAX) = N''
	DECLARE @v_Position INT
	DECLARE @v_NextPosition INT
	DECLARE @v_CommaCheck NVARCHAR(1) = RIGHT(@String, 1)
	IF @v_CommaCheck != @Delimiter
	BEGIN
		SET @String = @String + @Delimiter
	END
	SET @v_Position = CHARINDEX(@Delimiter, @String)
	SET @v_NextPosition = 1
	WHILE @v_Position !=  0
	BEGIN
		SET @v_NextString = SUBSTRING(@String, 1, @v_Position - 1)
		INSERT INTO @ValuableTable ([Value]) VALUES (@v_NextString)
		SET @String = SUBSTRING(@String ,@v_Position + 1, LEN(@String))
		SET @v_NextPosition = @v_Position
		SET @v_Position = CHARINDEX(@Delimiter, @String)
	END
	RETURN
END

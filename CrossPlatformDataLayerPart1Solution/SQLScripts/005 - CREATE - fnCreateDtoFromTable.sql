CREATE FUNCTION fnCreateDtoFromTable
(
	@TableName VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	
	DECLARE @ResultVar VARCHAR(MAX)

	SET @ResultVar = 'public class ' + @TableName + 'Dto' + CHAR(13) + CHAR(10);
	SET @ResultVar = @ResultVar + '{' + CHAR(13) + CHAR(10);

	-- Add the T-SQL statements to compute the return value here
	SELECT
		@ResultVar = @ResultVar + 'public ' + dbo.fnSQLTypeToCSharpType(typ.name, col.is_nullable) + ' ' + col.NAME + ' { get; set; }' + (case when col.is_nullable = 1 then ' // NULL' else ' // NOT NULL' END) + CHAR(13) + CHAR(10)
	FROM
		sys.columns AS col
		inner join sys.tables AS tab ON tab.object_id = col.object_id
		inner join sys.types AS typ ON typ.user_type_id = col.system_type_id
	WHERE
		tab.name = @TableName
	order by
		col.column_id

	SET @ResultVar = @ResultVar + '}';

	-- Return the result of the function
	RETURN @ResultVar

END
GO


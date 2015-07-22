CREATE FUNCTION [dbo].[fnSQLTypeToCSharpType]
(
	@Type varchar(max),
	@IsNullable bit
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @ResultVar varchar(max)

	SET @ResultVar = (CASE @Type
		WHEN 'int'              THEN 'int' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'image'            THEN 'byte[]'
		WHEN 'text'             THEN 'string'
		WHEN 'uniqueidentifier' THEN 'Guid' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'date'             THEN 'DateTime' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'time'             THEN 'DateTime' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'datetime2'        THEN 'DateTime' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'datetimeoffset'   THEN 'DateTime' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'tinyint'          THEN 'short' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'smallint'         THEN 'short' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'int'              THEN 'int' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'smalldatetime'    THEN 'DateTime' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'real'             THEN 'decimal' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'money'            THEN 'decimal' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'datetime'         THEN 'DateTime' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'float'            THEN 'decimal' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'sql_variant'      THEN 'string'
		WHEN 'ntext'            THEN 'string'
		WHEN 'bit'              THEN 'bool' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'decimal'			THEN 'decimal' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'numeric'		    THEN 'decimal' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'smallmoney'	    THEN 'decimal' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'bigint'		    THEN 'long' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'hierarchyid'		THEN 'int' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'geometry'			THEN 'string'
		WHEN 'geography'		THEN 'string'
		WHEN 'varbinary'		THEN 'int' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'varchar'			THEN 'string'
		WHEN 'binary'			THEN 'int' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'char'				THEN 'string'
		WHEN 'timestamp'		THEN 'DateTime' + (CASE WHEN @IsNullable = 1 THEN '? ' ELSE '' END)
		WHEN 'nvarchar'			THEN 'string'
		WHEN 'nchar'			THEN 'string'
		WHEN 'xml'				THEN 'string'
		WHEN 'sysname'			THEN 'string'
		ELSE 'string'
		END)

	RETURN @ResultVar

END

GO



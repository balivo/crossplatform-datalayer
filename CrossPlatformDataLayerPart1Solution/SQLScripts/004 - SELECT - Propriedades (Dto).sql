SELECT
	col.NAME as Coluna,
	'public ' +
	dbo.fnSQLTypeToCSharpType(typ.name, col.is_nullable) + ' ' +
	col.NAME +
	' { get; set; }' +
	(case when col.is_nullable = 1 then ' // NULL' else ' // NOT NULL' END)
	as Propriedades
FROM
	sys.columns AS col
	inner join sys.tables AS tab ON tab.object_id = col.object_id
	inner join sys.types AS typ ON typ.user_type_id = col.system_type_id
WHERE
	tab.name = 'Pais'
order by
	col.column_id
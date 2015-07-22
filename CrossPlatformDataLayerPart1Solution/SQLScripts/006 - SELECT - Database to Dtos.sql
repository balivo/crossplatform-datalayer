SELECT
	tab.name as Tabela,
	dbo.fnCreateDtoFromTable(tab.name) as Classe
FROM
	sys.tables AS tab
where
	tab.name <> 'sysdiagrams'
USE [MyCompare1]

select 'INSERT INTO [File] (ProcessId, Drive, [Path], [Name], Extension, ContractIndex) VALUES (''' +
Convert(nvarchar(100), ProcessId) + ''', ''' + 
[Drive] + ''', ''' + 
[Path] + ''', ''' + 
[Name] + ''', ' + 
isnull('''' + Convert(nvarchar(100), [ContractIndex]) + '''',	'NULL') + ') '
from [Files]
order by Drive, [Path], [Name]
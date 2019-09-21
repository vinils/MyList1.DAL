USE [MyCompare1]

select 'INSERT INTO [DATA] (ProcessId, Path) VALUES (''' +
Convert(nvarchar(100), ProcessId) + ''', ''' + 
[Path] + ''') '
from [Directories]
ORDER BY [ProcessId], [Path]
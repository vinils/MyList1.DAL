USE [MyCompare1]

SELECT 'INSERT INTO [FilesHash] (ProcessId, FullName, [Hash]) VALUES (''' +
Convert(nvarchar(100), ProcessId) + ''', ''' + 
[FullName] + ''', ''' + 
Convert(nvarchar(100), ProcessId) + ''')'
from [FilesHash]
ORDER BY ProcessId, [FullName], [Hash]
FROM vinils/mssql-server-windows-developer

ENV startdate="_" \
    attach_dbs="[]" \
    ACCEPT_EULA="_" \
    sa_password_path="C:\ProgramData\Docker\secrets\sa-password"
	
# escape=`

COPY . .\app
WORKDIR .\app

#SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

##bug - Sqlcmd: Error: Microsoft ODBC Driver 13 for SQL Server : TCP Provider: No connection could be made because the target machine actively refused it
#RUN sqlcmd -S localhost,1433 -i .\CREATE_DATABASE_MyCompare1_MS.sql ; \
RUN do { $count = $count + 1; $healfcheck = (sqlcmd -S localhost,1433 -i .\CREATE_DATABASE_MyCompare1_MS.sql); Start-Sleep -Seconds 10 } while(!$healfcheck -and $count -ne 60) ;

RUN sqlcmd -S localhost,1433 -i .\CREATE_TABLE_Directories.sql ; \
		sqlcmd -S localhost,1433 -i .\CREATE_TABLE_Files.sql ; \
		sqlcmd -S localhost,1433 -i .\CREATE_TABLE_FilesHash.sql ; 

#		Remove-Item -Recurse -Force .\DataDW ; \
#		Remove-Item -Recurse -Force .\DataContext ; 

CMD .\start -sa_password $env:sa_password -ACCEPT_EULA $env:ACCEPT_EULA -attach_dbs \"$env:attach_dbs\" -Verbose

#docker build . -t vinils/mylist1-dal
#docker run -it -d -p 1433:1433 -e sa_password=Password -e ACCEPT_EULA=Y vinils/mylist1-dal

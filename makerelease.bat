rmdir Release /s /q
mkdir Release
mkdir Release\plugin
mkdir Release\server
rem Build plugin
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe .\VStats-plugin\VStats-plugin.csproj /P:Configuration=Release
robocopy .\VStats-plugin\bin\Release\ .\Release\plugin *.dll *.ini /xf ScriptHookVDotNet.dll
rem Build server
cd VStats-server-go
copy ..\VStats-plugin\VStats-plugin.ini .\VStats-plugin.ini
go build 
cd ..
robocopy .\VStats-server-go\ .\Release\server /E /xf *.go build.bat
rem Copy license and readme
copy README.md .\Release\README.txt
copy LICENSE .\Release\LICENSE.txt

pause

@call dotnet restore -v m ..\

@if ERRORLEVEL 1 (
echo Error! Restoring dependencies failed.
exit /b 1
) else (
echo Restoring dependencies was successful.
)

@set project=..\src\server\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Server failed.
exit /b 1
)
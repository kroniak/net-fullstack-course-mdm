@call dotnet test -c Release /p:CollectCoverage=true /p:Exclude="[xunit.*]*" /p:CoverletOutputFormat=lcov /p:CoverletOutput=../../lcov ../test/server/
@if ERRORLEVEL 1 (
echo Error! Build Server failed.
exit /b 1
)
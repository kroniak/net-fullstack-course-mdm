@call dotnet test -c Release /p:CollectCoverage=true /p:Threshold=70 /p:Exclude="[xunit.*]*" ..\test\server\
@if ERRORLEVEL 1 (
echo Error! Tests for Server failed.
exit /b 1
)
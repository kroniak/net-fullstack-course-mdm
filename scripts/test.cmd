@call dotnet test -c Release /p:CollectCoverage=true /p:Threshold=90 /p:Exclude="[xunit.*]*" ..\test\server\Server.Test
@if ERRORLEVEL 1 (
echo Error! Tests for Server failed.
exit /b 1
)
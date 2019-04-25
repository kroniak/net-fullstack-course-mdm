FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app
COPY . .
RUN dotnet restore && dotnet publish -c Release -o out 
FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/out .
RUN ls -la
EXPOSE 59722
ENTRYPOINT ["dotnet","1.dll"]

FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet test
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/src/server/AlfaBank.WebApi/out/ .
ENV ASPNETCORE_URL=http://+:5002
EXPOSE 5002/tcp
ENTRYPOINT ["dotnet", "AlfaBank.WebApi.dll"]
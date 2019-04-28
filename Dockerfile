FROM microsoft/dotnet:2.2-sdk AS build

# copy
WORKDIR /app
COPY . .
RUN ./scripts/test.sh &&\
 ./scripts/build.sh
 
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "AlfaBank.WebApi.dll"]
FROM microsoft/dotnet:2.2-sdk AS build

# copy
WORKDIR /app
COPY . .
RUN dotnet restore -v m "net-fullstack-course-app.sln"
RUN ./scripts/test.sh && \
  ./scripts/build.sh

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
HEALTHCHECK --interval=10m --timeout=5s \
  CMD curl --fail http://localhost:5000/health/live || exit 1
ENTRYPOINT ["dotnet", "AlfaBank.WebApi.dll"] 
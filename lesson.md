# Курс Fullstack разработки на ASP.NET Core + React\Redux

## Блок 5. Инфраструктура

### Домашнее задание

1. Добавить Alfabank.StyleCop и отрефакторить приложение
2. Добавить Swagger OpenApi и провести необходимые изменения в проекте
3. Добавить конфигурацию из файлов и из переменных окружения
4. Добавить сквозное логирование в файлы во внешний volume контейнера через Serilog
5. Добавить HealthCheck endpoint, подумать над метриками, предоставленными приложением
6. \*\* Логирование осуществлять в ELK стек, поднятом в контейнере, рядом с контейнером приложения. Запускать весь стек через docker-compose
7. \*\* Добавить задание сервис через IHostedService

### Материалы для изучения дома

- <https://docs.docker.com/engine/examples/dotnetcore/> - Docker and ASP.NET Core
- <https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.2> - Swagger and ASP.NET Core
- <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2> - Configuration and ASP.NET Core
- <https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/analyzers?view=aspnetcore-2.2&tabs=visual-studio> - Analyzers and ASP.NET Core
- <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.2> - Logging and ASP.NET Core
- <https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/monitor-app-health> - HealthCheck and ASP.NET Core
- <https://github.com/serilog/serilog-aspnetcore> - Serilog and ASP.NET Core
- <https://www.elastic.co/elk-stack> - ELK Stack
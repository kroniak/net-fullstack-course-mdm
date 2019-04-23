# Курс Fullstack разработки на ASP.NET Core + React\Redux

## Блок 3. DI и Ioc pattern

### Домашнее задание

1. Переформатировать приложение:
  - разделить на подпроекты согласно ответственности (Web, Core, Data и т.д.)
  - netcoreapp приложение должно быть только WebApi
2. Middleware и сервисы, подгружаемые через DI, должны использовать свои расширения ```app.UseABC``` и ```services.AddABC```
3. Поправить все тесты и дописать необходимые новые, покрытие более 75% по строкам кода
4. Для mapping моделей и dto использовать `Automapper`
5. Подготовить вопросы по материалам прошлой лекции

### Материалы для изучения дома

- <https://1drv.ms/b/s!AswfoxlkvkXGgdI1UXmmiY0OvUXpcA> - C# 6.0 in a Nutshell
- <https://metanit.com/sharp/tutorial/3.1.php> - классы
- <https://metanit.com/sharp/tutorial/3.9.php> - интерфейсы
- <https://metanit.com/sharp/aspnet5/2.1.php> - класс `StartUp`
- <https://metanit.com/sharp/aspnet5/2.4.php> - middleware
- <https://metanit.com/sharp/aspnet5/2.18.php> - конвейер обработки запросов
- <https://enterprisecraftsmanship.com/2015/04/13/dto-vs-value-object-vs-poco/> - DTO vs POCO vs VO
- <https://github.com/AutoMapper/AutoMapper/> - AutoMapper
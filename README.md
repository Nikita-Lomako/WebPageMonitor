WebPageMonitor
Проект для мониторинга изменений на веб-страницах с использованием .NET и Entity Framework Core.

Настройка базы данных
1. Требования
Установленный .NET SDK (версия 6.0 или выше).

Локальная или удаленная база данных (например, SQL Server, SQLite, PostgreSQL).

2. Конфигурация строки подключения
Добавьте строку подключения к вашей базе данных в файл appsettings.json:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=ваш_сервер;Database=WebPageMonitor;User Id=ваш_пользователь;Password=ваш_пароль;"
  }
}
3. Создание миграций и применение к базе данных
Для работы с Entity Framework Core используйте следующие команды:

Создание новой миграции:
powershell
Add-Migration Названиемиграции -Project WebPageMonitor.Infrastructure -StartupProject WebPageMonitor.API
Применение миграций к базе данных:
powershell
Update-Database -Project WebPageMonitor.Infrastructure -StartupProject WebPageMonitor.API
4. Описание структуры базы данных
База данных содержит следующие таблицы:

WatchedPages
Id (int, первичный ключ)

Url (string) — URL отслеживаемой страницы.

Type (enum WebSiteType) — тип веб-сайта (например, Gismeteo).

PageVersions
Id (int, первичный ключ)

Content (JSON) — содержимое страницы на момент сохранения.

Timestamp (DateTime) — время сохранения версии.

WatchedPageId (int, внешний ключ) — ссылка на WatchedPages.

ChangeLogs
Id (int, первичный ключ)

DiffContent (JSON) — различия между версиями.

ChangeDate (DateTime) — дата изменения.

PageVersionId (int, внешний ключ) — ссылка на PageVersions.

5. Начальные данные (Seed Data)
При первом применении миграций в базу данных автоматически добавляются:

Тестовая страница для отслеживания (WatchedPage с URL https://www.gismeteo.by/...).

Две версии страницы (PageVersion) с примерами данных.

Лог изменений (ChangeLog), отражающий разницу между версиями.

6. Взаимодействие с контекстом
Пример использования контекста базы данных:

csharp
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    .Options;

using (var context = new AppDbContext(options))
{
    var pages = context.WatchedPages.Include(p => p.Versions).ToList();
}
Примечания
Для работы с миграциями убедитесь, что EF Core CLI установлен:

bash
dotnet tool install --global dotnet-ef
При изменении моделей или отношений обновите миграции командой Add-Migration.

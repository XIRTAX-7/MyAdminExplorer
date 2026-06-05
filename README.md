# SentinelPath

[![Build](https://github.com/XIRTAX-7/MyAdminExplorer/actions/workflows/build.yml/badge.svg)](https://github.com/XIRTAX-7/MyAdminExplorer/actions/workflows/build.yml)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4)
![Platform](https://img.shields.io/badge/platform-Windows-0078D4)
![License](https://img.shields.io/badge/license-MIT-green)

[English](README.en.md)

**SentinelPath** — Windows-приложение (WPF) для контролируемого доступа к файловой системе.

Администратор настраивает политики: кто может входить, в какие часы и дни, и к каким папкам разрешён доступ. Пользователи работают через встроенный проводник — запрещённые пути, расписание и правила чётности дня применяются автоматически.

Подходит для сценариев, где нужен простой файловый доступ с ролевой моделью без развёртывания полноценной инфраструктуры: учебные лаборатории, киоски, внутренние рабочие места с ограниченными правами.

## Возможности

- Роли **admin** и **user**, данные хранятся в `users.txt`
- Админ-панель: управление пользователями, расписанием и запрещёнными папками
- Файловый проводник с блокировкой путей
- Правила доступа в чётные и нечётные дни
- Временные окна по дате и часам
- PBKDF2-хеширование паролей с автоматической миграцией MD5
- CI-сборка и unit-тесты сервисного слоя

## Быстрый старт

### Требования

- Windows 10/11
- .NET Framework 4.8
- Visual Studio 2022 или MSBuild

### Сборка и запуск

```powershell
git clone https://github.com/XIRTAX-7/MyAdminExplorer.git
cd MyAdminExplorer
nuget restore MyAdminExplorer.sln
msbuild MyAdminExplorer.sln /p:Configuration=Release
.\MyAdminExplorer\bin\Release\MyAdminExplorer.exe
```

При первом запуске `users.txt` автоматически создаётся из `samples/users.txt.example`.

### Демо-учётные записи

| Логин | Пароль | Роль |
|-------|--------|------|
| admin | admin | Администратор |
| user | user | Пользователь |

### Тесты

```powershell
dotnet test MyAdminExplorer.Tests\MyAdminExplorer.Tests.csproj -c Release
```

## Архитектура

Подробная документация:

- [docs/architecture.md](docs/architecture.md) — окна, сервисы, диаграммы
- [docs/user-storage-format.md](docs/user-storage-format.md) — формат `users.txt`
- [docs/decisions.md](docs/decisions.md) — архитектурные решения (ADR)

```
MyAdminExplorer/
├── Services/          # Auth, access policy, repository, hashing
├── ViewModels/        # Login + user management
├── Infrastructure/    # AppPaths, FolderTreeHelper
├── Models/
└── Views (windows)    # WPF UI
```

## Авторский вклад (рефакторинг 2026)

Структурированная модернизация legacy-проекта под portfolio:

1. **Технический фундамент** — миграция 4.5.2 → 4.8, удаление Syncfusion и Xamarin.Forms
2. **Запуск из коробки** — sample users, автокопирование `users.txt`, иконки
3. **Безопасность** — убран backdoor admin/admin, PBKDF2 + миграция MD5, централизованная авторизация
4. **Архитектура** — тестируемый сервисный слой, `FolderTreeHelper`, лёгкий MVVM
5. **Качество** — xUnit-тесты, `.editorconfig`, GitHub Actions
6. **Презентация** — единая тема UI, документация, release-артефакт

Исправленные критические баги: неверный разделитель в `ChangePass`, некорректная проверка срока доступа, сломанные иконки дисков, дублирование записей в forbidden.

## Стек

- C# / WPF / .NET Framework 4.8
- xUnit + FluentAssertions
- GitHub Actions (MSBuild + tests + artifact)

## Ограничения

- Только Windows desktop (WPF)
- Пользователи хранятся в plain-text файле (без шифрования at rest)
- Нет автоматизированных UI-тестов
- Смешанные RU/EN строки в интерфейсе (наследие legacy)

## Roadmap

- [ ] Миграция на .NET 8 + современный WPF hosting
- [ ] SQLite или зашифрованное хранилище пользователей
- [ ] Полное покрытие MVVM
- [ ] Локализация через resx

## Лицензия

MIT — см. [LICENSE](LICENSE).

Оригинальный проект © 2017. Portfolio-редакция © 2017–2026.

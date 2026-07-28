# MyAdminExplorer

[![Build](https://github.com/XIRTAX-7/MyAdminExplorer/actions/workflows/build.yml/badge.svg)](https://github.com/XIRTAX-7/MyAdminExplorer/actions/workflows/build.yml)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4)
![Platform](https://img.shields.io/badge/platform-Windows-0078D4)
![License](https://img.shields.io/badge/license-MIT-green)

[Русский](README.md)

A Windows WPF desktop application with role-based authentication, an admin panel, and a file explorer restricted by schedule, day parity, and forbidden folders.

## Screenshots

| Login | Admin panel |
|-------|-------------|
| ![Login](docs/screenshots/login.png) | ![Admin](docs/screenshots/admin.png) |

| User management | Explorer |
|-----------------|----------|
| ![Users](docs/screenshots/users.png) | ![Explorer](docs/screenshots/explorer.png) |

## Features

- **admin** and **user** roles stored in `users.txt`
- Admin panel: user management, schedules, and forbidden folders
- File explorer with blocked paths
- Even/odd day access rules
- Date and hourly access windows
- PBKDF2 password hashing with automatic MD5 migration
- CI build and unit tests for the service layer

## Quick start

### Requirements

- Windows 10/11
- .NET Framework 4.8
- Visual Studio 2022 or MSBuild

### Build and run

```powershell
git clone https://github.com/XIRTAX-7/MyAdminExplorer.git
cd MyAdminExplorer
nuget restore MyAdminExplorer.sln
msbuild MyAdminExplorer.sln /p:Configuration=Release
.\MyAdminExplorer\bin\Release\MyAdminExplorer.exe
```

On first launch, `users.txt` is automatically created from `samples/users.txt.example`.

### Demo credentials

| Login | Password | Role |
|-------|----------|------|
| admin | admin | Administrator |
| user | user | Restricted user |

### Tests

```powershell
dotnet test MyAdminExplorer.Tests\MyAdminExplorer.Tests.csproj -c Release
```

## Architecture

Detailed documentation:

- [docs/architecture.md](docs/architecture.md) — windows, services, diagrams
- [docs/user-storage-format.md](docs/user-storage-format.md) — `users.txt` format
- [docs/decisions.md](docs/decisions.md) — architecture decision records (ADR)

```
MyAdminExplorer/
├── Services/          # Auth, access policy, repository, hashing
├── ViewModels/        # Login + user management
├── Infrastructure/    # AppPaths, FolderTreeHelper
├── Models/
└── Views (windows)    # WPF UI
```

## Author contributions (2026 refactor)

Structured legacy modernization for portfolio presentation:

1. **Build foundation** — migrated 4.5.2 → 4.8, removed Syncfusion and Xamarin.Forms
2. **Out-of-box experience** — sample users, auto-copy of `users.txt`, bundled icons
3. **Security** — removed admin/admin backdoor, PBKDF2 + MD5 migration, centralized auth
4. **Architecture** — testable service layer, `FolderTreeHelper`, lightweight MVVM
5. **Quality** — xUnit tests, `.editorconfig`, GitHub Actions
6. **Presentation** — unified UI theme, documentation, release artifact

Critical bugs fixed: wrong delimiter in `ChangePass`, incorrect access expiry check, broken drive icons, duplicate forbidden folder entries.

## Tech stack

- C# / WPF / .NET Framework 4.8
- xUnit + FluentAssertions
- GitHub Actions (MSBuild + tests + artifact)

## Limitations

- Windows desktop only (WPF)
- Plain-text user store (no encryption at rest)
- No automated UI tests
- Mixed RU/EN UI strings from legacy screens

## Roadmap

- [ ] Migrate to .NET 8 + modern WPF hosting
- [ ] SQLite or encrypted user store
- [ ] Full MVVM coverage
- [ ] Localization via resx

## License

MIT — see [LICENSE](LICENSE).

Original project © 2017. Refactored portfolio edition © 2017–2026.

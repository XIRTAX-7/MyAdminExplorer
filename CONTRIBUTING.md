# Contributing to MyAdminExplorer

Thank you for your interest in improving this project.

## Workflow

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-change`
3. Make your changes with focused commits
4. Run tests locally:
   ```powershell
   msbuild MyAdminExplorer.sln /p:Configuration=Release
   dotnet test MyAdminExplorer.Tests\MyAdminExplorer.Tests.csproj -c Release
   ```
5. Open a pull request with a clear description and test plan

## Guidelines

- Keep business logic in `Services/` — views should stay thin
- Add or update unit tests for service-layer changes
- Follow `.editorconfig` formatting rules
- Do not commit secrets, local `users.txt`, or build outputs
- Prefer small, reviewable PRs over large rewrites

## Scope

This is a portfolio WPF desktop app. Out of scope for contributions unless discussed first:

- .NET 8 migration (tracked in roadmap)
- Web API / Docker packaging
- Full MVVM rewrite of every window

## Questions

Open an issue for design questions before starting large changes.

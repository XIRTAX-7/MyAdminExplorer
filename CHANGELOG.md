# Changelog

All notable changes to this project are documented in this file.

## [2.0.0] - 2026-06-06

### Added
- Service layer: `FileUserRepository`, `AuthService`, `AccessPolicyService`
- PBKDF2 password hashing with automatic MD5 migration on login
- xUnit test project covering repository, auth, access policy, and hasher
- GitHub Actions CI (build, test, release artifact)
- Documentation: architecture, storage format, ADR decisions
- Sample `users.txt` auto-copy on first launch
- Unified WPF theme and window titles

### Changed
- Target framework upgraded from .NET Framework 4.5.2 to 4.8
- Removed Syncfusion and Xamarin.Forms dependencies
- Replaced GPL-3.0 with MIT license

### Fixed
- `ChangePass` corrupting `users.txt` (wrong `:` delimiter)
- Access expiry check using full `DateTime` instead of day-only math
- Disk icon not loading in TreeView converter
- Hardcoded `admin/admin` backdoor removed from login form
- Duplicate entries in forbidden folder list

### Removed
- Syncfusion WPF controls
- Dead Xamarin.Forms dependency
- Empty catch blocks in critical paths

## [1.0.0] - 2017

- Initial WPF application with admin panel and restricted file explorer

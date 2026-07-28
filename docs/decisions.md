# Architecture decisions

## ADR-001: Stay on .NET Framework 4.8

**Context:** The project started on .NET Framework 4.5.2 WPF.

**Decision:** Upgrade to 4.8 instead of migrating to .NET 8 now.

**Rationale:**
- Minimal friction for Windows desktop portfolio demo
- PBKDF2 available via `Rfc2898DeriveBytes`
- CI on `windows-latest` with MSBuild is straightforward

**Consequences:** Modern SDK-style tooling is used only for tests; main app remains classic csproj.

---

## ADR-002: PBKDF2 with MD5 migration

**Context:** Original app stored MD5 hashes without salt.

**Decision:** Store new hashes as PBKDF2; verify MD5 for legacy rows; upgrade on login.

**Rationale:** Improves security without breaking existing `users.txt` files.

---

## ADR-003: Remove Syncfusion and Xamarin.Forms

**Context:** Syncfusion DLLs were referenced but barely used; Xamarin.Forms was a dead dependency blocking restore.

**Decision:** Remove commercial UI dependencies; use standard WPF DataGrid and LINQ.

**Rationale:** Repository must build on any machine without proprietary packages.

---

## ADR-004: Lightweight MVVM

**Context:** Full MVVM for eight windows would be high cost for a portfolio refactor.

**Decision:** Introduce ViewModels for login and user management only; keep other windows as thin code-behind calling services.

**Rationale:** Demonstrates pattern without over-engineering.

---

## ADR-005: File-based user store

**Context:** No database in original design.

**Decision:** Keep `users.txt` with a dedicated repository.

**Rationale:** Matches original scope; easy to inspect and test.

---

## ADR-006: MIT license

**Context:** Project was GPL-3.0.

**Decision:** Relicense to MIT for portfolio-friendly reuse.

# User storage format

Users are stored in a plain-text file `users.txt` next to the executable.

On first launch the app copies `samples/users.txt.example` if `users.txt` is missing.

## Line format

Each non-empty line represents one user:

```text
login|passwordHash|role|even|from|to|fromTime|toTime|folder1?folder2
```

| Field | Type | Description |
|-------|------|-------------|
| `login` | string | Unique username |
| `passwordHash` | string | PBKDF2 (`pbkdf2:...`) or legacy 32-char MD5 hex |
| `role` | byte | `0` = user, `1` = admin |
| `even` | bool | `true` = access on even calendar days only |
| `from` | date | Start date (`yyyy-MM-dd` invariant) |
| `to` | date | End date |
| `fromTime` | int | Allowed from hour (0–24) |
| `toTime` | int | Allowed to hour (0–24) |
| forbidden | list | Folder names/paths separated by `?` |

## Example

```text
admin|21232f297a57a5a743894a0e4a801fc3|1|true|2020-01-01|2030-12-31|0|24|
user|04f8996da763b7a969b1028ee3007569|0|true|2020-01-01|2030-12-31|0|24|Windows?Program Files
```

Demo credentials (sample file):

- **admin** / **admin**
- **user** / **user**

## Password hashes

New and migrated passwords use PBKDF2:

```text
pbkdf2:{iterations}${base64Salt}${base64Hash}
```

Legacy MD5 entries (32 hex characters) are still accepted and upgraded automatically when the user signs in.

## Notes

- Empty forbidden segment is allowed (`...|24|`).
- Malformed lines are skipped on load.
- The repository serializes dates with invariant culture.

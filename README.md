# FarmPlus Electronic Scale

Windows desktop scale software manual-entry patch project. Repository is public and intentionally excludes customer data and the original proprietary binary.

## Requested behavior
- **F1**: leave the original save operation untouched.
- **F2**: replace the former "Save 2" keyboard shortcut with clearing `frmCan.txtSoluong` and focusing the same input; the Save 2 **button** still exists.
- Typing consecutive digits into either `txtSoluong` or `txtSoluong2` inserts exactly one decimal separator before the last digit (e.g. `1234 -> 123.4`; `12560 -> 1256.0`).
- Use manual data entry only; no hardware COM regression test was requested.

## State, 2026-10-09
A **test build** of a patched `Chuongtrinhcan.exe` has been produced from the user's supplied original binary in a private conversation artifact, together with the original application support files and a database-preserving installer. **The executable and database are NOT stored in this public repository.**

The binary patch is exact-build-specific (original SHA-256: `e53f72afa992cd76400e7e3bdb22c7630ac918bf0ba568174ba25be3582c59e7`). The test-build SHA-256 is `8859ee18f9f1bdebd14ef38b1843bc0c57d5ca062611d3aa28e796b5fcf640ba`.

Changes to the compiled .NET executable:
1. `frmCan_KeyDown` F2 branch clears/focuses first manual input instead of invoking `btnSave2`. The F1 branch remains byte-for-byte unchanged.
2. The two `txtSoluong*_TextChanged` handlers format keyboard entry with the decimal point and update their respective scale display labels; text caret is restored to end after formatting.
3. The `frmCan` constructor selects `en-US` for the scale-form UI thread because legacy `Double.Parse/TryParse` use the current culture.

## Verification limits
- Static checks: managed PE32/CLI metadata and method body branch destinations, preserved resource section, 298 untouched methods.
- **14 simulated IL-interpreter behavior scenarios passed** (normal digit entry, backspace, paste, F2 reset/focus, F1 routing).
- **Not yet verified:** actually launching the patched EXE on Windows, Windows keyboard focus, Access MDB persistence, receipt printing, rollback installation, and real user acceptance. Do not call this production-ready.
- The current green Windows CI workflow tests only the independent formatter in `src/ScaleInput`, **not** the patched application.

## Security and release hygiene
Never push the original database (`App/sys/data.mdb`), private records, registration material, credentials, or the uploaded ZIP to this public repository. Use synthetic test fixtures. Real database backups remain on the user's machine.

## Remaining acceptance steps
1. Launch the patched program on Windows from an extracted folder containing all original DLL/config dependencies.
2. Enable manual mode using the existing on-screen workflow.
3. Check input `1234 -> 123.4` and `12560 -> 1256.0`, the caret position, F2 clear/focus, and F1 unchanged.
4. Verify F1 saves the **correct decimal value** to a disposable MDB file, and verify printed receipts.
5. Test the upgrade script preservation/backup and restore path.
6. Only then label the EXE as a production release.

## Current CI
`.github/workflows/windows-ci.yml` runs on `windows-latest` and tests the standalone formatter, not the original binary or installer.

# FarmPlus Electronic Scale

Windows desktop scale software manual-entry patch project. Repository is public and intentionally excludes customer data and the original proprietary binary.

## Download the Windows test upgrade

- **GitHub Release:** https://github.com/ronganminh/farmplus-electronic-scale/releases/tag/v2.0.0-test
- **Direct ZIP:** https://github.com/ronganminh/farmplus-electronic-scale/releases/download/v2.0.0-test/FarmPlus_Manual_v2_PUBLIC_TEST.zip
- **Windows CI verification (PASS):** https://github.com/ronganminh/farmplus-electronic-scale/actions/runs/37884832547
- **ZIP SHA-256:** `647bc140fa15982b3bee298b9bad23c0ee1d7d42f224c1f42b342b84c7598cd8`

**Upgrade an existing Windows installation only.** The public ZIP intentionally omits the Access `data.mdb` database and bundled font. Close the original app, extract the ZIP, launch `CAI_DAT_FARMPLUS_V2.bat`, and select the folder with the existing installation. The installer backs up the local EXE and database before updating. Run `PHUC_HOI_EXE_CU.bat` to roll back.

**Warning:** This is a *test prerelease*. GitHub-hosted Windows CI verified 9 standalone number-formatting cases, managed EXE metadata and hash, ZIP structure and PowerShell installer syntax. It has **not** acceptance-tested the actual GUI, F1/F2 keypresses, Access database saves or receipt printing.

## Requested behavior
- **F1**: leave the original save operation untouched.
- **F2**: replace the former "Save 2" keyboard shortcut with clearing `frmCan.txtSoluong` and focusing the same input; the Save 2 **button** still exists.
- Typing consecutive digits into either `txtSoluong` or `txtSoluong2` inserts exactly one decimal separator before the last digit (e.g. `1234 -> 123.4`; `12560 -> 1256.0`).
- Use manual data entry only; no hardware COM regression test was requested.

## State, 2026-10-09
A **test build** of a patched `Chuongtrinhcan.exe` has been produced from the original binary, with a database-preserving installer and Windows-test ZIP attached to the public prerelease above. **The executable and database are NOT committed as source files in this repository.**

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
- `.github/workflows/windows-ci.yml`: build + 9 tests for the standalone formatter.
- `.github/workflows/publish-windows-test.yml`: checks the staged ZIP on `windows-latest`, verifies the patched EXE's checksum and managed metadata, validates the installer syntax, tests the independent formatter, and publishes a draft prerelease if verification passes. **Does not test the production UI, database I/O, or printing.**

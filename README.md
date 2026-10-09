# FarmPlus Electronic Scale

Public development workspace for the FarmPlus manual-entry scale software.

## Target behavior
- Keep the current **F1** save behavior.
- Replace the existing **F2** save-2 shortcut with **clear current manual weight input, focus the input, ready to type again**.
- Insert a decimal point before the final digit when typing digits continuously: `1234` -> `123.4`; `12560` -> `1256.0`.
- Preserve accurate numeric values in save, reports and printouts.
- No physical COM/FTDI scale tests are required for the manual-entry workflow.

## Status
**Preparation only. This repository DOES NOT contain a modified Chuongtrinhcan.exe.**
The original application is a compiled 32-bit .NET Framework Windows program, with no project source supplied. The formatting code in this repository is independently tested; it is **not yet integrated** into the original application's UI, keyboard events or database.

## Security
Do not commit the original Access database (`data.mdb`), real records, passwords, license keys, logs, or the complete original installation ZIP. Use a synthetic fixture only. The CI workflow has no access to production data.

## Next engineering steps
1. Inspect/decompile original executable locally on Windows; map the manual-entry TextBox, F1/F2 KeyDown handlers, and conversion/storage paths.
2. Choose tested binary patch or reconstructed WinForms project based on the actual assemblies. Preserve a hash-verified pristine backup.
3. Wire the formatter to the specific input (not all TextBoxes), handle Backspace/Delete/paste/selection and avoid recursive TextChanged calls.
4. Replace F2 action with clear + focus only; keep F1 behavior unchanged.
5. Integrate test database and print-path tests, then verify UI automation in an interactive Windows session. GitHub-hosted Actions may not expose a reliable interactive desktop for GUI tests.
6. Publish a modified EXE only after integration tests succeed. Currently CI tests a **standalone formatting component**, not the production executable.

## CI
GitHub Actions: `.github/workflows/windows-ci.yml` runs `dotnet run --project tests/ScaleInput.SmokeTests` on `windows-latest` and compiles the test harness.

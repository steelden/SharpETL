@echo off

call "%VS100COMNTOOLS%\vsvars32.bat"
echo ---------------------------------

set SNKFILE=..\src\SharpETL.snk
if not exist "%SNKFILE%" (
    echo Generating new strong name key file at "%SNKFILE%".
    sn -q -k "%SNKFILE%"
    if errorlevel 1 (
        echo Generation failed. See error message for details.
    ) else (
        echo Generated successfully.
    )
) else (
    echo Strong name key file already exists at "%SNKFILE%".
)


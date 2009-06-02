
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /if "BandObjectLib.dll"
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /if "Interop.ShDocVw.dll"
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /if "MatrixControl.dll"

IF EXIST "%SYSTEMROOT%\Microsoft.NET\Framework" "%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\regasm" "MatrixControl.dll"
IF EXIST "%SYSTEMROOT%\Microsoft.NET\Framework64" "%SYSTEMROOT%\Microsoft.NET\Framework64\v2.0.50727\regasm" "MatrixControl.dll"

taskkill /f /im explorer.exe
start explorer.exe

IF EXIST "%SYSTEMROOT%\Microsoft.NET\Framework" "%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\regasm" /u "MatrixControl.dll"
IF EXIST "%SYSTEMROOT%\Microsoft.NET\Framework64" "%SYSTEMROOT%\Microsoft.NET\Framework64\v2.0.50727\regasm" /u "MatrixControl.dll"

"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /u "MatrixControl"
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /u "Interop.ShDocVw"
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /u "BandObjectLib"

taskkill /f /im explorer.exe
start explorer.exe

"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /if "BandObjectLib.dll"
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /if "Interop.ShDocVw.dll"
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\gacutil" /if "MatrixControl.dll"
rem "%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\regasm" "MatrixControl.dll"
"%SYSTEMROOT%\Microsoft.NET\Framework64\v2.0.50727\regasm" "MatrixControl.dll"

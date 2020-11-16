%~dp0\installutil.exe  %~dp0\bin\Debug\WindowsService.exe
Net Start GameService
sc config GameService start= auto
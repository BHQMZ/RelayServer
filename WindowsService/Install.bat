%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe  %~p0WindowsService.exe
Net Start GameService
sc config GameService start= auto
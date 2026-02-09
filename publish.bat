dotnet publish "BPSR-FDPS/BPSR-FDPS.csproj" -r win-x64 -c Release -o ./publish /p:PublishSingleFile=true /p:PublishTrimmed=false /p:TrimMode=Link /p:IncludeAllContentForSelfExtract=false --self-contained false
move "publish\BPSR-FDPS.exe" "publish\BPSR-FDPS.exe"
copy "BPSR-FDPS\Data" "publish\Data"

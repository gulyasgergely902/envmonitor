del /f /s /q artifact 1>nul
dotnet build -c Release --self-contained false
dotnet test

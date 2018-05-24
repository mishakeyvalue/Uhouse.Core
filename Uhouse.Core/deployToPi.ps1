dotnet publish -r linux-arm -o publish

pscp -pw raspberry -r publish pi@192.168.0.100:uhouse-core-publish
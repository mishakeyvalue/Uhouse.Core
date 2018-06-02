dotnet publish -r linux-arm -o publish

pscp -pw raspberry -r publish pi@192.168.0.103:uhouse-core-publish
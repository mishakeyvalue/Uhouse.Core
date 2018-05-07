dotnet publish -r linux-arm -o publish

pscp -pw raspberry -r publish pi@raspberrypi.local:uhouse-core-publish
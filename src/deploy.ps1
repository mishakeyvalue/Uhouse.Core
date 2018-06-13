dotnet publish -r linux-arm -o publish

pscp -pw raspberry -r .\Uhouse.Core.Web\publish\ pi@192.168.0.102:Uhouse.Core.Web


# [Install]
# WantedBy=multi-user.target

# [Unit]
# Description=sensor data reader

# [Service]
# Type=simple
# WorkingDirectory=/home/pi/programming
# ExecStart=/home/pi/Uhouse.Core.Web/publish/Uhouse.Core.Web
# Restart=always

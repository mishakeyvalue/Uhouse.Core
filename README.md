# Uhouse.Core

## To find your Pi address from widnwows

```
nmap -sn 192.168.0.0/24
```

## To enable tunnel from Pi ( connected via Ethernet ) to local port

Example:
 To tunnel server on the Pi ( local IP address `169.254.23.140` ) listening on `8000`
 to `localhost:8080` do:


1. Connection->SSH->Tunnels
2. Source port: `8080`
3. Destination: `169.254.23.140:8000`
4. Configure session and open connection

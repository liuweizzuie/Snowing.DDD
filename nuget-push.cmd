del -f -q *.nupkg

nuget pack

nuget push *.nupkg  -Source http://192.168.56.254:8080

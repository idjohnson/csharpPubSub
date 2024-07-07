# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY  /out .
ENTRYPOINT ["dotnet", "csharp-subscriber.dll"]
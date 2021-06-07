FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
EXPOSE 80
COPY ./publish .
ENTRYPOINT ["dotnet", "LagoVista.IoT.Simulator.Runtime.Portal.dll"]
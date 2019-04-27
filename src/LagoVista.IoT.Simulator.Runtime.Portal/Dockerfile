FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
COPY ./publish .
ENTRYPOINT ["dotnet", "LagoVista.IoT.Simulator.Runtime.Portal.dll"]
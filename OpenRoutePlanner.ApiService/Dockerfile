FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
RUN mkdir -p /app/data
RUN chmod -R 777 /app/data
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OpenRoutePlanner.ApiService/OpenRoutePlanner.ApiService.csproj", "OpenRoutePlanner.ApiService/"]
RUN dotnet restore "OpenRoutePlanner.ApiService/OpenRoutePlanner.ApiService.csproj"
COPY . .
WORKDIR "/src/OpenRoutePlanner.ApiService"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OpenRoutePlanner.ApiService.dll"]

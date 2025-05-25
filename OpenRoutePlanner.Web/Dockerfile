FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OpenRoutePlanner.Web/OpenRoutePlanner.Web.csproj", "OpenRoutePlanner.Web/"]
RUN dotnet restore "OpenRoutePlanner.Web/OpenRoutePlanner.Web.csproj"
COPY . .
WORKDIR "/src/OpenRoutePlanner.Web"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OpenRoutePlanner.Web.dll"]

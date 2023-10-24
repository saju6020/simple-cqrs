#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/BlogWeb/BlogWeb.csproj", "src/BlogWeb/"]
COPY ["src/Platform.Infrastructure.Host.WebApi/Platform.Infrastructure.Host.WebApi.csproj", "src/Platform.Infrastructure.Host.WebApi/"]
COPY ["src/EndpointRoleFeatureMap/EndpointRoleFeatureMap.csproj", "src/EndpointRoleFeatureMap/"]
COPY ["src/Platform.Infrastructure.Common/Platform.Infrastructure.Common.csproj", "src/Platform.Infrastructure.Common/"]
COPY ["src/ServiceRegistration.Provider/ServiceRegistration.Provider.csproj", "src/ServiceRegistration.Provider/"]
COPY ["src/Platform.Infrastructure.Core/Platform.Infrastructure.Core.csproj", "src/Platform.Infrastructure.Core/"]
RUN dotnet restore "src/BlogWeb/BlogWeb.csproj"
COPY . .
WORKDIR "/src/src/BlogWeb"
RUN dotnet build "BlogWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogWeb.dll"]
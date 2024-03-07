#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UserAccessManagmentService/UAM.WebService.csproj", "src/UserAccessManagmentService/"]
COPY ["src/Authentication/Authentication.csproj", "src/Authentication/"]
COPY ["src/Platform.Infrastructure.Common/Platform.Infrastructure.Common.csproj", "src/Platform.Infrastructure.Common/"]
COPY ["src/Platform.Infrastructure.Host.WebApi/Platform.Infrastructure.Host.WebApi.csproj", "src/Platform.Infrastructure.Host.WebApi/"]
COPY ["src/Platform.Infrastructure.Core/Platform.Infrastructure.Core.csproj", "src/Platform.Infrastructure.Core/"]
COPY ["src/UAM.Common/UAM.Common.csproj", "src/UAM.Common/"]
COPY ["src/UAM.Database/UAM.Database.csproj", "src/UAM.Database/"]
COPY ["src/UAM.Dtos/UAM.Dtos.csproj", "src/UAM.Dtos/"]
COPY ["src/UAM.Mapper/UAM.Mapper.csproj", "src/UAM.Mapper/"]
COPY ["src/UAM.Domain/UAM.Domain.csproj", "src/UAM.Domain/"]
COPY ["src/Repository/Platform.Infrastructure.Repository.csproj", "src/Repository/"]
COPY ["src/Platform.Infrastructure.Repository.Contract/Platform.Infrastructure.Repository.Contract.csproj", "src/Platform.Infrastructure.Repository.Contract/"]
COPY ["src/Validation.FluentValidation/Validation.FluentValidationProvider.csproj", "src/Validation.FluentValidation/"]
RUN dotnet restore "src/UserAccessManagmentService/UAM.WebService.csproj"
COPY . .
WORKDIR "/src/src/UserAccessManagmentService"
RUN dotnet build "UAM.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UAM.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UAM.WebService.dll"]
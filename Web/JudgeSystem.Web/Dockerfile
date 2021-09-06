#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Web/JudgeSystem.Web/JudgeSystem.Web.csproj", "Web/JudgeSystem.Web/"]
COPY ["Web/JudgeSystem.Web.InputModels/JudgeSystem.Web.InputModels.csproj", "Web/JudgeSystem.Web.InputModels/"]
COPY ["Services/JudgeSystem.Services.Mapping/JudgeSystem.Services.Mapping.csproj", "Services/JudgeSystem.Services.Mapping/"]
COPY ["Data/JudgeSystem.Data.Models/JudgeSystem.Data.Models.csproj", "Data/JudgeSystem.Data.Models/"]
COPY ["Data/JudgeSystem.Data.Common/JudgeSystem.Data.Common.csproj", "Data/JudgeSystem.Data.Common/"]
COPY ["JudgeSystem.Common/JudgeSystem.Common.csproj", "JudgeSystem.Common/"]
COPY ["Web/JudgeSystem.Web.Infrastructure/JudgeSystem.Web.Infrastructure.csproj", "Web/JudgeSystem.Web.Infrastructure/"]
COPY ["Workers/JudgeSystem.Workers.Common/JudgeSystem.Workers.Common.csproj", "Workers/JudgeSystem.Workers.Common/"]
COPY ["Web/JudgeSystem.Web.ViewModels/JudgeSystem.Web.ViewModels.csproj", "Web/JudgeSystem.Web.ViewModels/"]
COPY ["Web/JudgeSystem.Web.Dtos/JudgeSystem.Web.Dtos.csproj", "Web/JudgeSystem.Web.Dtos/"]
COPY ["Services/JudgeSystem.Services.Data/JudgeSystem.Services.Data.csproj", "Services/JudgeSystem.Services.Data/"]
COPY ["Workers/JudgeSystem.Executors/JudgeSystem.Executors.csproj", "Workers/JudgeSystem.Executors/"]
COPY ["Services/JudgeSystem.Services/JudgeSystem.Services.csproj", "Services/JudgeSystem.Services/"]
COPY ["Workers/JudgeSystem.Compilers/JudgeSystem.Compilers.csproj", "Workers/JudgeSystem.Compilers/"]
COPY ["Data/JudgeSystem.Data/JudgeSystem.Data.csproj", "Data/JudgeSystem.Data/"]
COPY ["Services/JudgeSystem.Services.Messaging/JudgeSystem.Services.Messaging.csproj", "Services/JudgeSystem.Services.Messaging/"]
RUN dotnet restore "Web/JudgeSystem.Web/JudgeSystem.Web.csproj"
COPY . .
WORKDIR "/src/Web/JudgeSystem.Web"
RUN dotnet build "JudgeSystem.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JudgeSystem.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JudgeSystem.Web.dll"]
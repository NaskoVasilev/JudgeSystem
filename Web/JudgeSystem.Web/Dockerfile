FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
# COPY ["LinixInstallTest/LinixInstallTest.csproj", "LinixInstallTest/"]
COPY ./*.sln ./

COPY Data/JudgeSystem.Data.Common/JudgeSystem.Data.Common.csproj ./Data/JudgeSystem.Data.Common/
COPY Data/JudgeSystem.Data.Models/JudgeSystem.Data.Models.csproj ./Data/JudgeSystem.Data.Models/
COPY Data/JudgeSystem.Data/JudgeSystem.Data.csproj ./Data/JudgeSystem.Data/
COPY Documentation/TestGenerator/SolutionTransformer/SolutionTransformer.csproj ./Documentation/TestGenerator/SolutionTransformer/
COPY Documentation/TestGenerator/TestGenerator/TestGenerator.csproj ./Documentation/TestGenerator/TestGenerator/
COPY JudgeSystem.Common/JudgeSystem.Common.csproj ./JudgeSystem.Common/
COPY Services/JudgeSystem.Services.Data/JudgeSystem.Services.Data.csproj ./Services/JudgeSystem.Services.Data/
COPY Services/JudgeSystem.Services.Mapping/JudgeSystem.Services.Mapping.csproj ./Services/JudgeSystem.Services.Mapping/
COPY Services/JudgeSystem.Services.Messaging/JudgeSystem.Services.Messaging.csproj ./Services/JudgeSystem.Services.Messaging/
COPY Services/JudgeSystem.Services/JudgeSystem.Services.csproj ./Services/JudgeSystem.Services/
COPY Tests/JudgeSystem.Services.Data.Tests/JudgeSystem.Services.Data.Tests.csproj ./Tests/JudgeSystem.Services.Data.Tests/
COPY Tests/JudgeSystem.Services.Tests/JudgeSystem.Services.Tests.csproj ./Tests/JudgeSystem.Services.Tests/
COPY Tests/JudgeSystem.Web.Tests/JudgeSystem.Web.Tests.csproj ./Tests/JudgeSystem.Web.Tests/
COPY Tests/Sandbox/Sandbox.csproj ./Tests/Sandbox/
COPY Web/JudgeSystem.Web.Dtos/JudgeSystem.Web.Dtos.csproj ./Web/JudgeSystem.Web.Dtos/
COPY Web/JudgeSystem.Web.Infrastructure/JudgeSystem.Web.Infrastructure.csproj ./Web/JudgeSystem.Web.Infrastructure/
COPY Web/JudgeSystem.Web.InputModels/JudgeSystem.Web.InputModels.csproj ./Web/JudgeSystem.Web.InputModels/
COPY Web/JudgeSystem.Web.ViewModels/JudgeSystem.Web.ViewModels.csproj ./Web/JudgeSystem.Web.ViewModels/
####### VVVVVVVVV
COPY Web/JudgeSystem.Web/JudgeSystem.Web.csproj ./Web/JudgeSystem.Web/
####### ^^^^^^^^^
COPY Workers/JudgeSystem.Checkers/JudgeSystem.Checkers.csproj ./Workers/JudgeSystem.Checkers/
COPY Workers/JudgeSystem.Compilers/JudgeSystem.Compilers.csproj ./Workers/JudgeSystem.Compilers/
COPY Workers/JudgeSystem.Executors/JudgeSystem.Executors.csproj ./Workers/JudgeSystem.Executors/
COPY Workers/JudgeSystem.Workers.Common/JudgeSystem.Workers.Common.csproj ./Workers/JudgeSystem.Workers.Common/

RUN dotnet restore "Web/JudgeSystem.Web/JudgeSystem.Web.csproj"
COPY . .
WORKDIR "/src/Web/JudgeSystem.Web/"
RUN dotnet build "JudgeSystem.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JudgeSystem.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt update
RUN apt -y install g++

RUN apt install -y firejail

#FROM rockylinux/rockylinux
#RUN /sbin/adduser sandbox
#USER sandbox

ENTRYPOINT ["dotnet", "JudgeSystem.Web.dll"]
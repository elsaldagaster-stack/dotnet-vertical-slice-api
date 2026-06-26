FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/TaskFlow.Api/TaskFlow.Api.csproj", "src/TaskFlow.Api/"]
RUN dotnet restore "src/TaskFlow.Api/TaskFlow.Api.csproj"
COPY . .
WORKDIR "/src/src/TaskFlow.Api"
RUN dotnet build "TaskFlow.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskFlow.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]

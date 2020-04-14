FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["RJM.BackgroundTasks.csproj", "RJM.BackgroundTasks/"]
RUN dotnet restore "RJM.BackgroundTasks/RJM.BackgroundTasks.csproj"
COPY . "RJM.BackgroundTasks/"
WORKDIR "/src/RJM.BackgroundTasks"
RUN dotnet build "RJM.BackgroundTasks.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RJM.BackgroundTasks.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RJM.BackgroundTasks.dll"]

# Specify the base image for the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Specify the base image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project files into the container
COPY ["NLayer.API/NLayer.API.csproj", "NLayer.API/"]
COPY ["NLayer.Caching/NLayer.Caching.csproj", "NLayer.Caching/"]
COPY ["NLayer.Service/NLayer.Service.csproj", "NLayer.Service/"]
COPY ["NLayer.Repository/NLayer.Repository.csproj", "NLayer.Repository/"]
COPY ["NLayer.Core/NLayer.Core.csproj", "NLayer.Core/"]

# Restore the NuGet packages
RUN dotnet restore "NLayer.API/NLayer.API.csproj"
COPY . .

# Build the application
WORKDIR "/src/NLayer.API"
RUN dotnet build "NLayer.API.csproj" -c Release -o /app/build

# Specify the base image for the published application
FROM build AS publish

# Publish the application
RUN dotnet publish "NLayer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Specify the final base image for the runtime environment
FROM base AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=publish /app/publish .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "NLayer.API.dll"]

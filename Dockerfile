#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the ASP.NET runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the nuget.config file if you have one
COPY ["nuget.config", "."]
# Copy the project file and restore dependencies
COPY ["BlazorTicketmasterApp.csproj", "."]
RUN dotnet restore "./BlazorTicketmasterApp.csproj"

# Copy the remaining source code and build the application
COPY . .
WORKDIR "/src/."
RUN dotnet build "./BlazorTicketmasterApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BlazorTicketmasterApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the runtime image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorTicketmasterApp.dll"]

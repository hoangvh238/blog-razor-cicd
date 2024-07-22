# Use the official ASP.NET 5 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set the environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Use the .NET 5 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy the solution file
COPY Blog.sln ./

# Copy all the project files and restore dependencies
COPY BusinessLayer/BusinessLayer.csproj BusinessLayer/
COPY Core/Core.csproj Core/
COPY DataAccessLayer/DataAccessLayer.csproj DataAccessLayer/
COPY EntityLayer/EntityLayer.csproj EntityLayer/

RUN dotnet restore

# Copy the remaining source code and build the application
COPY . .

WORKDIR /src/Core
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY Blog.sln ./

COPY BusinessLayer/BusinessLayer.csproj BusinessLayer/
COPY Core/Core.csproj Core/
COPY DataAccessLayer/DataAccessLayer.csproj DataAccessLayer/
COPY EntityLayer/EntityLayer.csproj EntityLayer/

RUN dotnet restore

COPY . .

WORKDIR /src/Core
RUN dotnet build -c Release -o /app/build

RUN dotnet tool install --global dotnet-ef

ENV PATH="${PATH}:/root/.dotnet/tools"

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.dll"]

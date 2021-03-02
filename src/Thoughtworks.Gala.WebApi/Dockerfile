# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:3.1.406-alpine3.12 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY src/Thoughtworks.Gala.WebApi/*.csproj ./Thoughtworks.Gala.WebApi/
RUN dotnet restore Thoughtworks.Gala.WebApi/Thoughtworks.Gala.WebApi.csproj

# copy everything else and build app
COPY src/Thoughtworks.Gala.WebApi/. ./Thoughtworks.Gala.WebApi/
WORKDIR /src/Thoughtworks.Gala.WebApi
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:3.1.12-alpine3.12
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "Thoughtworks.Gala.WebApi.dll" ]
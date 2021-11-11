FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /BurgerAPI

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet build


EXPOSE 3005

RUN dotnet run
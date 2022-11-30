FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY . .
RUN rm -rf Backend/bin Backend/obj
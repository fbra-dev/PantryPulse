FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Backend.WebApi/Backend.WebApi.csproj", "Backend.WebApi/"]
RUN dotnet restore "Backend.WebApi/Backend.WebApi.csproj"
COPY . .
WORKDIR "/src/Backend.WebApi"
RUN dotnet build "Backend.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

# Install dockerize to the final stage
ENV DOCKERIZE_VERSION v0.7.0
RUN apt-get update && apt-get install -y wget
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dockerize", "-wait", "tcp://db:5432", "-timeout", "1m", "dotnet", "Backend.WebApi.dll"]

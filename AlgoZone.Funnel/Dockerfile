FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["AlgoZone.Funnel/AlgoZone.Funnel.csproj", "AlgoZone.Funnel/"]
COPY ["AlgoZone.Funnel.Datalayer.RabbitMQ/AlgoZone.Funnel.Datalayer.RabbitMQ.csproj", "AlgoZone.Funnel.Datalayer.RabbitMQ/"]
COPY ["AlgoZone.Funnel.Datalayer.Elasticsearch/AlgoZone.Funnel.Datalayer.Elasticsearch.csproj", "AlgoZone.Funnel.Datalayer.Elasticsearch/"]
COPY ["AlgoZone.Funnel.Datalayer.Binance/AlgoZone.Funnel.Datalayer.Binance.csproj", "AlgoZone.Funnel.Datalayer.Binance/"]
COPY ["AlgoZone.Funnel.Businesslayer/AlgoZone.Funnel.Businesslayer.csproj", "AlgoZone.Funnel.Businesslayer/"]
COPY ["AlgoZone.Funnel.Exceptions/AlgoZone.Funnel.Exceptions.csproj", "AlgoZone.Funnel.Exceptions/"]
RUN dotnet restore "AlgoZone.Funnel/AlgoZone.Funnel.csproj"
COPY . .
WORKDIR "/src/AlgoZone.Funnel"

ARG GH_USERNAME
ARG GH_TOKEN
RUN dotnet nuget add source --username $GH_USERNAME --password $GH_TOKEN --store-password-in-clear-text https://nuget.pkg.github.com/Algo-Zone/index.json --name github
RUN dotnet build "AlgoZone.Funnel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlgoZone.Funnel.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlgoZone.Funnel.dll"]

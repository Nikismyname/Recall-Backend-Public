FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

COPY ./src/. ./

RUN dotnet restore ./Recall.Web/*.csproj
RUN dotnet publish ./Recall.Web/*.csproj -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app

COPY --from=build-env /app/Recall.Web/out ./
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Recall.Web.dll

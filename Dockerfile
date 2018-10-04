FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app/
# EXPOSE 51740
# EXPOSE 44394


FROM microsoft/dotnet:2.1-sdk AS web-app-build
ARG configuration=Debug
WORKDIR /project/
COPY deps deps
COPY src src
RUN dotnet build src/Vivid.Web/Vivid.Web.csproj --configuration ${configuration}


FROM web-app-build AS publish
WORKDIR /project/
RUN dotnet publish src/Vivid.Web/Vivid.Web.csproj --configuration Release --output /app/


FROM base AS final
WORKDIR /app/
COPY --from=publish /app /app
CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet Vivid.Web.dll


FROM microsoft/dotnet:2.1-sdk AS solution-build
ARG configuration=Debug
WORKDIR /project/
COPY . .
RUN dotnet build Vivid.sln --configuration ${configuration}
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CloudPrint.Api/CloudPrint.Api.csproj", "src/CloudPrint.Api/"]
RUN dotnet restore "src/CloudPrint.Api/CloudPrint.Api.csproj"
COPY . .
RUN dotnet publish "src/CloudPrint.Api/CloudPrint.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CloudPrint.Api.dll"]

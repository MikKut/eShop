#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Order/Order.Host/Order.Host.csproj", "Order/Order.Host/"]
RUN dotnet restore "Order/Order.Host/Order.Host.csproj"
COPY . .
WORKDIR "/src/Order/Order.Host"
RUN dotnet build "Order.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Host.dll"]
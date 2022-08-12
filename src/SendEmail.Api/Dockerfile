FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SendEmail.Api/SendEmail.Api.csproj", "SendEmail.Api/"]
COPY ["SendEmail.Application/SendEmail.Application.csproj", "SendEmail.Application/"]
COPY ["SendEmail.Data/SendEmail.Data.csproj", "SendEmail.Data/"]
COPY ["SendEmail.Business/SendEmail.Business.csproj", "SendEmail.Business/"]
RUN dotnet restore "SendEmail.Api/SendEmail.Api.csproj"
COPY . .
WORKDIR "SendEmail.Api"
RUN dotnet build "SendEmail.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SendEmail.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SendEmail.Api.dll"]

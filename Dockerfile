#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["lab-api-aspnetcore-gql/CommanderGQL.csproj", "lab-api-aspnetcore-gql/"]
COPY ["lab-data-dotnetcore-ef/CommanderData.csproj", "lab-data-dotnetcore-ef/"]
RUN dotnet restore "lab-api-aspnetcore-gql/CommanderGQL.csproj"
COPY . .
WORKDIR "/src/lab-api-aspnetcore-gql"
RUN dotnet build "CommanderGQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommanderGQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommanderGQL.dll"]
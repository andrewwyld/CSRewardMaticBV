FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /app
COPY . .
RUN dotnet restore ./RewardMatic4000.sln
RUN dotnet publish -c Release -o out ./Console/ConsoleApp.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
CMD ["./Console"]
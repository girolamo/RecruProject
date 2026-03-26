FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/RecruProject.sln ./
COPY src/RecruProject.API/RecruProject.API.csproj RecruProject.API/
COPY src/RecruProject.Core/RecruProject.Core.csproj RecruProject.Core/
COPY src/RecruProject.Infrastructure/RecruProject.Infrastructure.csproj RecruProject.Infrastructure/
COPY src/RecruProject.Infrastructure.Tests/RecruProject.Infrastructure.Tests.csproj RecruProject.Infrastructure.Tests/

RUN dotnet restore RecruProject.sln

COPY . ./

RUN dotnet publish src/RecruProject.API/RecruProject.API.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "RecruProject.API.dll"]
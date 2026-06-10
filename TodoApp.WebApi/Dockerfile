FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /src
COPY . .

RUN dotnet restore "TodoApp.WebApi/TodoApp.WebApi.csproj"
RUN dotnet publish "TodoApp.WebApi/TodoApp.WebApi.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TodoApp.WebApi.dll"]
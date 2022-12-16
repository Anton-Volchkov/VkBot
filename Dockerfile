FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS build

COPY . .
RUN dotnet restore
WORKDIR "VkBot"
RUN dotnet build "VkBot.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "VkBot.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VkBot.dll"]
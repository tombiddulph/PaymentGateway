FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
# Copy csproj and restore as distinct layers
WORKDIR /src
#OPY *.csproj ./
#COPY ["./PaymentGateway.Api.csproj", "src/PaymentGateway.Api/"]
#COPY ["/src/PaymentGateway.Application/PaymentGateway.Application.csproj", "src/PaymentGateway.Application/"]
#COPY ["src/PaymentGateway.Data/PaymentGateway.Data.csproj", "src/PaymentGateway.Data/"]
#COPY ["src/PaymentGateway.Models/PaymentGateway.Models.csproj", "src/PaymentGateway.Models/"]

COPY . .
RUN dotnet restore "./PaymentGateway.Api.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]
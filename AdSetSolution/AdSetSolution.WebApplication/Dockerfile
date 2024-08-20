# Use a imagem SDK para desenvolvimento
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie os arquivos do projeto e restaure as dependências
COPY ["AdSetSolution.WebApplication/AdSetSolution.WebApplication.csproj", "AdSetSolution.WebApplication/"]
COPY ["AdSetSolution.Infrastructure/AdSetSolution.Infrastructure.csproj", "AdSetSolution.Infrastructure/"]
COPY ["AdSetSolution.Application/AdSetSolution.Application.csproj", "AdSetSolution.Application/"]
COPY ["AdSetSolution.Data/AdSetSolution.Domain.csproj", "AdSetSolution.Data/"]
COPY ["AdSetSolution.ServiceDefaults/AdSetSolution.ServiceDefaults.csproj", "AdSetSolution.ServiceDefaults/"]
RUN dotnet restore "./AdSetSolution.WebApplication/AdSetSolution.WebApplication.csproj"

# Copie o restante do código
COPY . .

# Instale o dotnet-watch para monitoramento
RUN dotnet tool install --global dotnet-watch

# Defina o PATH para incluir o dotnet-watch
ENV PATH="${PATH}:/root/.dotnet/tools"

# Execute dotnet-watch para desenvolvimento
WORKDIR "/src/AdSetSolution.WebApplication"
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]

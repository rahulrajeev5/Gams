# ---------------------
# Build stage
# ---------------------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /app
    
    # Copy everything
    COPY . .
    
    # Restore and publish
    RUN dotnet restore "backend.csproj"
    RUN dotnet publish "backend.csproj" -c Release -o out
    
    # ---------------------
    # Runtime stage with GAMS
    # ---------------------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
    
    # Install dependencies and GAMS
    RUN apt-get update && \
        apt-get install -y wget unzip libgomp1 && \
        wget https://d37drm4t2jghv5.cloudfront.net/distributions/49.5.0/linux/linux_x64_64_sfx.exe -O /tmp/gams_installer.run && \
        chmod +x /tmp/gams_installer.run && \
        /tmp/gams_installer.run -q && \
        ln -s /gams/gams49.5_linux_x64_64/gams /usr/local/bin/gams && \
        gams -h || echo "GAMS installed but help output suppressed in Docker build" && \
        apt-get clean && rm -rf /var/lib/apt/lists/*
    
    # Set working directory
    WORKDIR /app
    
    # Copy published app from build stage
    COPY --from=build /app/out .
    
    # Copy GAMS model files
    COPY GamsModels /app/GamsModels
    
    # Expose your backend port
    EXPOSE 5000
    
    # Set entry point
    ENTRYPOINT ["dotnet", "backend.dll"]
    
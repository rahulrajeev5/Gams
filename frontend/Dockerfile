# Runtime stage with GAMS
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

RUN apt-get update && \
    apt-get install -y wget unzip libgomp1 && \
    wget https://d37drm4t2jghv5.cloudfront.net/distributions/49.5.0/linux/linux_x64_64_sfx.exe -O /tmp/gams_installer.run && \
    chmod +x /tmp/gams_installer.run && \
    /tmp/gams_installer.run -q && \
    ln -s /gams/gams49.5_linux_x64_64/gams /usr/local/bin/gams && \
    echo "Listing /gams and /usr/local/bin:" && \
    ls -la /gams && \
    ls -la /usr/local/bin && \
    which gams && gams -h || echo "GAMS not found or not executable"

WORKDIR /app
COPY --from=build /app/out .
COPY GamsModels /app/GamsModels

EXPOSE 5000
ENTRYPOINT ["dotnet", "backend.dll"]

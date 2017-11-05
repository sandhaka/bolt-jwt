FROM microsoft/aspnetcore:2.0.0
ARG source
WORKDIR /app
EXPOSE 80
COPY bin/Debug/netcoreapp2.0/publish .
ENTRYPOINT /bin/bash -c "dotnet BoltJwt.dll"
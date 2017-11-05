FROM microsoft/aspnetcore:2.0.0
WORKDIR /app
EXPOSE 80
COPY obj/docker .
ENTRYPOINT /bin/bash -c "dotnet BoltJwt.dll"
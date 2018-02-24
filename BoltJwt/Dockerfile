FROM microsoft/aspnetcore:2.0.5
WORKDIR /app
EXPOSE 443

COPY obj/docker/release.tar /app
RUN tar -xf release.tar

ENTRYPOINT /bin/bash -c "dotnet BoltJwt.dll"
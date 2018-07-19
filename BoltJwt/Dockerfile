FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
EXPOSE 443

COPY obj/docker/release.tar /app
RUN tar -xf release.tar

ENTRYPOINT /bin/bash -c "dotnet BoltJwt.dll"
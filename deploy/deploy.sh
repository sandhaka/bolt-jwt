#!/bin/bash

# Clear web dest folder first
rm -r ../BoltJwt/wwwroot/*

# Build frontend
(cd ../web && ng build --env=prod)

# Include it into the project
cp ../web/dist/* ../BoltJwt/wwwroot/

# Cleaning
rm -r ../BoltJwt/obj/docker/*

# Build backend and publish the project to the build docker folder
(cd ../BoltJwt && dotnet publish -o obj/docker)

# Compression
(cd ../BoltJwt/obj/docker && tar -zcvf release.tar .)

# Output file:
(cd ../BoltJwt/obj/docker; echo 'Deploy done - Output file: '; eval 'ls -al')
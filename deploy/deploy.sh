#!/bin/bash

# Clear web dest folder first
rm -r ../BoltJwt/wwwroot/*

# Build frontend
(cd ../web && ng build --env=prod)

# Include it into the project
cp ../web/dist/* ../BoltJwt/wwwroot/

# Build backend and publish the project to the build docker folder
(cd ../BoltJwt && dotnet publish -o obj/docker)
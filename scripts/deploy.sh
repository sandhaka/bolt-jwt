#!/bin/bash

# Deployment based on published docker images
# Deploy the latest 'dev' version of the project

# Copy deployment files
scp -i rsa/deploy_rsa.pem ../docker-compose.yml root@94.177.201.179:~/deployed/BoltJwt/

# Run 
ssh -i rsa/deploy_rsa.pem root@94.177.201.179 'SQL_CONNECTION_STRING="Server=sql.data,1433;User Id=sa;Password=Password&1;Initial Catalog=boltJwt.data" SQL_PASSWORD="Password&1" docker-compose -f ~/deployed/BoltJwt/docker-compose.yml pull && docker-compose -f ~/deployed/BoltJwt/docker-compose.yml -up -d'

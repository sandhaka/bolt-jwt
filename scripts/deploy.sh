#!/bin/bash

# Deployment based on published docker images
# Deploy the latest 'dev' version of the project

# Copy deployment files
scp -i rsa/deploy_rsa.pem ../docker-compose.yml root@94.177.201.179:~/deployed/BoltJwt/

# Run 
ssh -i rsa/deploy_rsa.pem root@94.177.201.179 'docker-compose -f ~/deployed/BoltJwt/docker-compose.yml up --build -d'
#!/bin/bash

# Create the structure folders if not exists
ssh -i rsa/deploy_rsa.pem root@94.177.201.179 'mkdir -p deployed/ && mkdir -p deployed/BoltJwt && \
    mkdir -p deployed/BoltJwt/BoltJwt && mkdir -p deployed/BoltJwt/BoltJwt/certs'

# Copy certificates
scp -r -i rsa/deploy_rsa.pem ../BoltJwt/certs root@94.177.201.179:~/deployed/BoltJwt/BoltJwt/
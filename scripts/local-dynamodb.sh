#!/usr/bin/env bash

set -euo pipefail

# move to repo base folder
cd "$(dirname "${BASH_SOURCE[0]}")/../.." || exit 1;

docker run --rm \
  -it \
  --name dynamodb-local \
  -p 8000:8000 \
  -v "$(pwd)/.dynamodb:/home/dynamodblocal/data" \
  -w /home/dynamodblocal \
  amazon/dynamodb-local \
    -jar DynamoDBLocal.jar -sharedDb -optimizeDbBeforeStartup -dbPath ./data
  

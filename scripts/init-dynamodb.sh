#!/usr/bin/env bash

set -euo pipefail

# move to repo base folder
cd "$(dirname "${BASH_SOURCE[0]}")/../.." || exit 1;

docker run --rm \
  -it \
  --name dynamodb-init \
  --net="host" \
  banst/awscli \
    dynamodb create-table \
      --table-name Galas \
      --attribute-definitions \
        AttributeName=GalaId,AttributeType=S \
        AttributeName=Name,AttributeType=S \
        AttributeName=Year,AttributeType=N \
        AttributeName=Programs, AttributeType=M \
      --key-schema \
        AttributeName=GalaId,KeyType=HASH \
        AttributeName=Year,KeyType=RANGE \
      --provisioned-throughput \
        ReadCapacityUnits=10,WriteCapacityUnits=5 \
      --endpoint-url http://localhost:8000
      
docker run --rm \
  -it \
  --name dynamodb-init \
  --net="host" \
  banst/awscli \
    dynamodb create-table \
      --table-name Programs \
      --attribute-definitions \
        AttributeName=ProgramId,AttributeType=S \
        AttributeName=Name,AttributeType=S \
        AttributeName=Description,AttributeType=S \
        AttributeName=Performers,AttributeType=M \
      --key-schema \
        AttributeName=ProgramId,KeyType=HASH \
        AttributeName=Name,KeyType=RANGE \
      --provisioned-throughput \
        ReadCapacityUnits=10,WriteCapacityUnits=5 \
      --endpoint-url http://localhost:8000
      
docker run --rm \
  -it \
  --name dynamodb-init \
  --net="host" \
  banst/awscli \
    dynamodb create-table \
      --table-name Performers \
      --attribute-definitions \
        AttributeName=PerformerId,AttributeType=S \
        AttributeName=Name,AttributeType=S \
      --key-schema \
        AttributeName=PerformerId,KeyType=HASH \
      --provisioned-throughput \
        ReadCapacityUnits=10,WriteCapacityUnits=5 \
      --endpoint-url http://localhost:8000

#!/usr/bin/env bash

set -euo pipefail

# move to repo base folder
cd "$(dirname "${BASH_SOURCE[0]}")/../.." || exit 1;

AWS_ACCESS_KEY_ID=AKIAIOSFODNN7EXAMPLE
AWS_SECRET_ACCESS_KEY=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
AWS_DEFAULT_REGION=us-west-2

docker run --rm \
  -it \
  --name dynamodb-init \
  --env AWS_ACCESS_KEY_ID="${AWS_ACCESS_KEY_ID}" \
  --env AWS_SECRET_ACCESS_KEY="${AWS_SECRET_ACCESS_KEY}" \
  --env AWS_DEFAULT_REGION="${AWS_DEFAULT_REGION}" \
  --net="host" \
  banst/awscli \
    dynamodb create-table \
      --table-name Galas \
      --attribute-definitions \
        AttributeName=GalaId,AttributeType=S \
      --key-schema \
        AttributeName=GalaId,KeyType=HASH \
      --provisioned-throughput \
        ReadCapacityUnits=10,WriteCapacityUnits=5 \
      --region us-west-2 \
      --endpoint-url http://localhost:8000
      
docker run --rm \
  -it \
  --name dynamodb-init \
  --env AWS_ACCESS_KEY_ID="${AWS_ACCESS_KEY_ID}" \
  --env AWS_SECRET_ACCESS_KEY="${AWS_SECRET_ACCESS_KEY}" \
  --env AWS_DEFAULT_REGION="${AWS_DEFAULT_REGION}" \
  --net="host" \
  banst/awscli \
    dynamodb create-table \
      --table-name Programs \
      --attribute-definitions \
        AttributeName=ProgramId,AttributeType=S \
      --key-schema \
        AttributeName=ProgramId,KeyType=HASH \
      --provisioned-throughput \
        ReadCapacityUnits=10,WriteCapacityUnits=5 \
      --endpoint-url http://localhost:8000
      
docker run --rm \
  -it \
  --name dynamodb-init \
  --env AWS_ACCESS_KEY_ID="${AWS_ACCESS_KEY_ID}" \
  --env AWS_SECRET_ACCESS_KEY="${AWS_SECRET_ACCESS_KEY}" \
  --env AWS_DEFAULT_REGION="${AWS_DEFAULT_REGION}" \
  --net="host" \
  banst/awscli \
    dynamodb create-table \
      --table-name Performers \
      --attribute-definitions \
        AttributeName=PerformerId,AttributeType=S \
      --key-schema \
        AttributeName=PerformerId,KeyType=HASH \
      --provisioned-throughput \
        ReadCapacityUnits=10,WriteCapacityUnits=5 \
      --endpoint-url http://localhost:8000
      
docker run --rm \
  -it \
  --name dynamodb-init \
  --env AWS_ACCESS_KEY_ID="${AWS_ACCESS_KEY_ID}" \
  --env AWS_SECRET_ACCESS_KEY="${AWS_SECRET_ACCESS_KEY}" \
  --env AWS_DEFAULT_REGION="${AWS_DEFAULT_REGION}" \
  --net="host" \
  banst/awscli \
    dynamodb list-tables \
      --endpoint-url http://localhost:8000

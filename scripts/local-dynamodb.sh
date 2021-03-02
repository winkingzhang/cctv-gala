#!/usr/bin/env bash

set -euo pipefail

# move to repo base folder
cd "$(dirname "${BASH_SOURCE[0]}")/../.." || exit 1;

docker run --rm \
  -it \
  --name dynamodb-local \
  --net "host" \
  --publish 8000:8000 \
  --volume $(pwd)/.dynamodb:/dynamodb/data \
  --workdir /dynamodb \
  amazon/dynamodb-local \
    -sharedDb -optimizeDbBeforeStartup -dbPath ./data
  
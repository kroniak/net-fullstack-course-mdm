#!/usr/bin/env bash
set -euo pipefail

command -v docker >/dev/null 2>&1 || {
    echo >&2 "This script requires the docker to be installed"
    exit 1
}

SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

docker build -t alfabank-service:latest "$SCRIPT_ROOT/.."

docker build -t alfabank-service-cli:latest "$SCRIPT_ROOT/../src/client"

docker build -t alfabank-balancer:latest "$SCRIPT_ROOT/../src/nginx"

docker rmi -f $(docker images -q --filter "dangling=true")

#!/usr/bin/env bash
set -euo pipefail

command -v docker >/dev/null 2>&1 || {
    echo >&2 "This script requires the docker to be installed"
    exit 1
}

SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

docker run -d -p 5001:5000 -v /tmp/alfabank-service-logs:/app/logs --name myapp alfabank-service
docker run -d -p 3000:3000 --name myapp-cli alfabank-service-cli

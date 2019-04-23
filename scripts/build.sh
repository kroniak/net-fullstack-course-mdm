#!/usr/bin/env bash
set -euo pipefail

command -v dotnet >/dev/null 2>&1 || {
    echo >&2 "This script requires the dotnet core sdk tooling to be installed"
    exit 1
}

SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

NETSTANDARD_SLN="${SCRIPT_ROOT}/../net-fullstack-course-app.sln"

echo "!!WARNING!! This script only builds netstandard and netcoreapp targets"

dotnet restore -v m "${NETSTANDARD_SLN}"

dotnet publish -c Release -o "${SCRIPT_ROOT}/../out" "${SCRIPT_ROOT}/../src/server/" 

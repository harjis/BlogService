#!/usr/bin/env bash

set -o errexit
set -o pipefail
set -o nounset

printf "Uninstall DB\n\n"
./db/uninstall.sh

printf "\n\nUninstall kafka\n\n"
./kafka/uninstall.sh
#!/bin/bash

helm uninstall postgresql

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
bash ${__dir}/pvc-delete.sh
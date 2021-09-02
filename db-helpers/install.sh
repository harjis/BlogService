#!/bin/bash

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
bash ${__dir}/pvc-apply.sh

helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install postgresql bitnami/postgresql -f k8s-postgres/values.yaml
#!/usr/bin/env bash

# Not set on purpose. We want to try to uninstall everything even if previous commands fail.
# This is useful when install succeeds only partially.
#set -o errexit
set -o pipefail
set -o nounset

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

kubectl delete -f "${__dir}"/k8s/values.yaml

helm uninstall confluent-operator

kubectl delete namespace primary-kafka
kubectl delete namespace cfk
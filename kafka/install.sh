#!/usr/bin/env bash

set -o errexit
set -o pipefail
set -o nounset

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

kubectl create namespace primary-kafka
kubectl create namespace cfk
kubectl config set-context --current --namespace cfk

helm repo add confluentinc https://packages.confluent.io/helm
helm repo update
helm upgrade --install confluent-operator \
  confluentinc/confluent-for-kubernetes \
  --set namespaceList="{primary-kafka,cfk}" \
  --namespace cfk \
  --set namespaced=true

kubectl apply -f "${__dir}"/k8s/values.yaml
kubectl config set-context --current --namespace default
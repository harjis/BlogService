#!/bin/bash

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

kubectl apply -f "${__dir}"/k8s/seed.yaml

# TODO This dont seem to work
kubectl wait --for=condition=complete --timeout=600s job/db-seed

kubectl delete job db-seed
#!/bin/bash

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

kubectl apply -f "${__dir}"/k8s/db-pvc.yaml
kubectl apply -f "${__dir}"/k8s/db.yaml

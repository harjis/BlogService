#!/bin/bash

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

kubectl delete -f "${__dir}"/k8s/db.yaml
kubectl delete -f "${__dir}"/k8s/db-pvc.yaml

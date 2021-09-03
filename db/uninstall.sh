#!/bin/bash

helm uninstall postgresql

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
kubectl delete -f "${__dir}"/k8s/db-pvc.yaml

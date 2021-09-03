#!/bin/bash

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

helm install blog-service-kafka confluentinc/cp-helm-charts -f "${__dir}"/k8s/values.yaml
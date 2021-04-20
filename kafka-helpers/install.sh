#!/bin/bash

helm install blog-service-kafka confluent/cp-helm-charts -f k8s-kafka/values.yaml
# helm install blog-service-kafka confluent/cp-helm-charts
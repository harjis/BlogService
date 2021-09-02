#!/bin/bash

helm install blog-service-kafka confluentinc/cp-helm-charts -f k8s-kafka/values.yaml
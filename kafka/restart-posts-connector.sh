#!/bin/bash

kubectl exec \
  -it deployment/blog-service-kafka-cp-kafka-connect \
  -- curl -X POST "http://localhost:8083/connectors/source-post-service/restart"

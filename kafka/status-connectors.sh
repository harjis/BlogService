#!/bin/bash

status=$(
  kubectl exec \
    -it deployment/blog-service-kafka-cp-kafka-connect \
    -- curl -s "http://localhost:8083/connectors?expand=info&expand=status"
)

echo "$status" | jq '. | to_entries[] | [ .value.info.type, .key, .value.status.connector.state,.value.status.tasks[].state,.value.info.config."connector.class"]|join(":|:")' |
  column -s : -t | sed 's/\"//g' | sort

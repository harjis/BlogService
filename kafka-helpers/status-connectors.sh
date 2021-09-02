#!/bin/bash

kubectl exec \
  -it blog-service-kafka-cp-kafka-connect-964796598-ncmfc \
  -- bash
#  -- curl -s "http://blog-service-kafka-cp-kafka-connect:8083/connectors?expand=info&expand=status")

# echo "$status" | jq

#echo "$status" | jq '. | to_entries[] | [ .value.info.type, .key, .value.status.connector.state,.value.status.tasks[].state,.value.info.config."connector.class"]|join(":|:")' |
#  column -s : -t | sed 's/\"//g' | sort

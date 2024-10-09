#!/bin/bash

kubectl exec -it zookeeper-0 -- \
kafka-topics --zookeeper primary-kafka.primary-kafka.svc.cluster.local:2181 --list
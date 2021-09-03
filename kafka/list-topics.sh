#!/bin/bash

kubectl exec -it blog-service-kafka-cp-zookeeper-0 -- \
kafka-topics --zookeeper blog-service-kafka-cp-zookeeper-headless:2181 --list
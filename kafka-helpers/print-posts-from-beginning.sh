#!/bin/bash

kubectl exec -it blog-service-kafka-cp-zookeeper-0 -- \
kafka-console-consumer \
--bootstrap-server blog-service-kafka-cp-kafka-headless:9092 \
--topic Post.events \
--from-beginning
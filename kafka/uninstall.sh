#!/bin/bash

helm uninstall blog-service-kafka

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

"${DIR}"/clear-kafka-data.sh
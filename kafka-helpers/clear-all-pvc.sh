#!/bin/bash

kubectl get pvc --no-headers=true | awk '/pvc|kafka/{print $1}' | xargs kubectl delete pvc
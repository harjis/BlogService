#!/bin/bash

# Clear all pvc
echo "Clear all pvc"
kubectl get pvc --no-headers=true | awk '/kafka/{print $1}' | xargs kubectl delete pvc

# Clear all released pv
echo "Clear all released pv"
kubectl get pv | grep Released | awk '$1 {print$1}' | while read -r vol; do kubectl delete pv/"${vol}"; done
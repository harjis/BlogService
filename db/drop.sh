#!/bin/bash

if [ $# -eq 0 ]; then
  echo "No arguments supplied: ./drop.sh posts"
  exit 1
fi

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [ $1 == "posts" ]; then
  # PostsService
  kubectl apply -f "${__dir}"/k8s/drop-posts.yaml
  echo "Waiting posts drop"
  kubectl wait --for=condition=complete --timeout=600s job/db-drop-posts
  echo "Delete posts job"
  kubectl delete job db-drop-posts
elif [ $1 == "comments" ]; then
  # CommentsService
  kubectl apply -f "${__dir}"/k8s/drop-comments.yaml
  echo "Waiting comments drop"
  kubectl wait --for=condition=complete --timeout=600s job/db-drop-comments
  echo "Delete comments job"
  kubectl delete job db-drop-comments
else
  echo "Give posts or comments as argument: ./drop.sh posts"
fi

#!/bin/bash

__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# PostsService
kubectl apply -f "${__dir}"/k8s/migrate-posts.yaml
echo "Waiting posts migration"
kubectl wait --for=condition=complete --timeout=600s job/db-migrate-posts
echo "Delete posts job"
kubectl delete job db-migrate-posts

# CommentsService
kubectl apply -f "${__dir}"/k8s/migrate-comments.yaml
echo "Waiting comments migration"
kubectl wait --for=condition=complete --timeout=600s job/db-migrate-comments
echo "Delete comments job"
kubectl delete job db-migrate-comments
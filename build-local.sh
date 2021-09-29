#!/bin/bash

VERSION=0.1.0
SHA=$(git rev-parse HEAD)

eval $(minikube -p minikube docker-env)

# PostService-DbMigrations
docker build \
-t d0rka/post-service-migrations:latest \
-t d0rka/post-service-migrations:"$SHA" \
-t d0rka/post-service-migrations:$VERSION \
-f ./PostService.DbMigrations/Dockerfile .

# CommentService.DbMigrations
docker build \
-t d0rka/comment-service-migrations:latest \
-t d0rka/comment-service-migrations:"$SHA" \
-t d0rka/comment-service-migrations:$VERSION \
-f ./CommentService.DbMigrations/Dockerfile .
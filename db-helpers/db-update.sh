kubectl exec -it deployment/post-service-deployment -- dotnet ef database update
kubectl exec -it deployment/comment-service-deployment -- dotnet ef database update

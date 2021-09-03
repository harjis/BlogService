You need to have these tools installed:
- skaffold https://skaffold.dev/docs/install/ 
- helm https://helm.sh/docs/intro/install/

# Setup

0. Secret
```
kubectl create secret generic post-db-password --from-literal postgresql-password=my_pgpassword
```


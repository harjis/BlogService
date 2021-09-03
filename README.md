You need to have these tools installed:
- skaffold https://skaffold.dev/docs/install/ 
- helm https://helm.sh/docs/intro/install/

# Setup

0. Secret
```
kubectl create secret generic post-db-password --from-literal postgresql-password=my_pgpassword
```

1. Install postgres
```shell
./db/install.sh
```

2. Build images to local repository
```shell
./build-local.sh
```

3. Migrate db
```shell
./db/migrate.sh
```

4. Install kafka
```shell
./kafka/install.sh
```

4. Start application
```shell
skaffold dev
```

5. Navigate to [minikube ip](http://192.168.64.3/posts)
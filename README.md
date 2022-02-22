You need to have these tools installed:
- skaffold https://skaffold.dev/docs/install/ 
- helm https://helm.sh/docs/intro/install/

# Setup

0. Secret
```
kubectl create secret generic pgpassword --from-literal postgres-password=my_pgpassword
```

## Bring up local environment

1. Install dependencies
```shell
./up.sh
```

2. Start application
```shell
skaffold dev
```
3. Navigate to [minikube ip](http://192.168.64.3/posts)
## Teardown
1. Shutdown skaffold
2. Remove dependencies
```shell
./down.sh
```

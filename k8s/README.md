# Kubernetes Manifests

This directory contains all Kubernetes manifests for deploying GigaShop to Azure Kubernetes Service (AKS).

## Directory Structure

```
k8s/
├── namespace.yaml                    # Namespace definition
├── databases/                        # Database deployments
│   ├── catalogdb-deployment.yaml     # PostgreSQL for Catalog
│   ├── basketdb-deployment.yaml      # PostgreSQL for Basket
│   └── orderdb-deployment.yaml       # SQL Server for Orders
├── infrastructure/                   # Infrastructure services
│   ├── redis-deployment.yaml         # Redis cache
│   └── rabbitmq-deployment.yaml      # RabbitMQ message broker
├── services/                         # Microservices
│   ├── catalog-api-deployment.yaml   # Catalog API
│   ├── basket-api-deployment.yaml    # Basket API
│   ├── order-api-deployment.yaml     # Order API
│   └── discount-grpc-deployment.yaml # Discount gRPC service
├── gateway/                          # API Gateway
│   └── apigateway-deployment.yaml    # YARP API Gateway
├── frontend/                         # Frontend application
│   └── web-deployment.yaml           # React web app
├── ingress/                          # Ingress configuration
│   └── ingress.yaml                  # NGINX Ingress
└── kustomization.yaml                # Kustomize configuration
```

## Deployment Order

1. **Namespace**: Creates the `gigashop` namespace
2. **Databases**: PostgreSQL and SQL Server instances
3. **Infrastructure**: Redis and RabbitMQ
4. **Services**: All microservices (Catalog, Basket, Order, Discount)
5. **Gateway**: API Gateway
6. **Frontend**: Web application
7. **Ingress**: External access configuration

## Quick Deploy

```bash
# Deploy everything
kubectl apply -k .

# Or deploy step by step
kubectl apply -f namespace.yaml
kubectl apply -f databases/
kubectl apply -f infrastructure/
kubectl apply -f services/
kubectl apply -f gateway/
kubectl apply -f frontend/
kubectl apply -f ingress/
```

## Resource Requirements

### Minimum Cluster Requirements

- **Nodes**: 3 nodes minimum
- **Node Size**: Standard_D2s_v3 or larger
- **Total CPU**: ~4 vCPUs
- **Total Memory**: ~8 GB RAM
- **Storage**: ~50 GB

### Per-Service Resources

| Service | CPU Request | CPU Limit | Memory Request | Memory Limit |
|---------|-------------|-----------|----------------|--------------|
| Catalog API | 200m | 500m | 256Mi | 512Mi |
| Basket API | 200m | 500m | 256Mi | 512Mi |
| Order API | 200m | 500m | 256Mi | 512Mi |
| Discount gRPC | 100m | 300m | 128Mi | 256Mi |
| API Gateway | 200m | 500m | 256Mi | 512Mi |
| Web | 100m | 300m | 128Mi | 256Mi |
| CatalogDB | 250m | 500m | 256Mi | 512Mi |
| BasketDB | 250m | 500m | 256Mi | 512Mi |
| OrderDB | 500m | 1000m | 2Gi | 4Gi |
| Redis | 100m | 200m | 128Mi | 256Mi |
| RabbitMQ | 200m | 500m | 256Mi | 512Mi |

## Configuration

### Update ACR Name

Before deploying, update the ACR name in all deployment files:

```bash
# Replace <ACR_NAME> with your actual ACR name
find . -type f -name "*.yaml" -exec sed -i 's/<ACR_NAME>/youracr/g' {} +
```

### Update Domain Names

Update domain names in `ingress/ingress.yaml`:

```yaml
spec:
  tls:
  - hosts:
    - gigashop.yourdomain.com        # Change this
    - api.gigashop.yourdomain.com    # Change this
```

### Update Environment Variables

Each service has environment variables that can be customized:

**Example: Catalog API**
```yaml
env:
- name: ASPNETCORE_ENVIRONMENT
  value: "Production"
- name: ConnectionStrings__Database
  value: "Host=catalogdb;Port=5432;Database=CatalogDb;Username=postgres;Password=postgres"
```

## Storage

All stateful services use PersistentVolumeClaims (PVCs):

- **Storage Class**: `managed-premium` (Azure Premium SSD)
- **Access Mode**: `ReadWriteOnce`
- **Sizes**:
  - Databases: 10Gi each
  - Redis: 5Gi
  - RabbitMQ: 5Gi

### Change Storage Class

To use standard storage (cost-effective):

```yaml
spec:
  storageClassName: managed-standard  # Instead of managed-premium
```

## Autoscaling

All microservices have HorizontalPodAutoscaler (HPA) configured:

- **Min Replicas**: 2
- **Max Replicas**: 10
- **CPU Target**: 70%
- **Memory Target**: 80%

View HPA status:

```bash
kubectl get hpa -n gigashop
```

## Health Checks

All services have health probes configured:

### Liveness Probe
Checks if the container is running. If it fails, Kubernetes restarts the container.

### Readiness Probe
Checks if the container is ready to accept traffic. If it fails, the pod is removed from service endpoints.

## Networking

### Service Types

- **ClusterIP**: Internal services (databases, microservices)
- **LoadBalancer**: Ingress controller (external access)

### Service Discovery

Services communicate using Kubernetes DNS:

```
<service-name>.<namespace>.svc.cluster.local
```

Example:
- `catalogdb.gigashop.svc.cluster.local`
- `catalog-api.gigashop.svc.cluster.local`

## Secrets

Secrets are stored as Kubernetes Secret objects. For production, consider:

1. **Azure Key Vault**: Integrate with AKS
2. **Sealed Secrets**: Encrypt secrets in Git
3. **External Secrets Operator**: Sync from external secret stores

### Update Secrets

```bash
# Edit secret
kubectl edit secret catalogdb-secret -n gigashop

# Or create from file
kubectl create secret generic my-secret \
  --from-file=./secret-file \
  -n gigashop
```

## Monitoring

### View Logs

```bash
# View logs for a specific pod
kubectl logs -n gigashop <pod-name>

# Follow logs
kubectl logs -n gigashop <pod-name> -f

# View logs from all containers in a pod
kubectl logs -n gigashop <pod-name> --all-containers
```

### Check Status

```bash
# Get all resources
kubectl get all -n gigashop

# Get pods with more details
kubectl get pods -n gigashop -o wide

# Describe a resource
kubectl describe pod -n gigashop <pod-name>
```

### Events

```bash
# View events
kubectl get events -n gigashop --sort-by='.lastTimestamp'
```

## Troubleshooting

### Pod Not Starting

```bash
# Check pod status
kubectl describe pod -n gigashop <pod-name>

# Check logs
kubectl logs -n gigashop <pod-name>

# Check previous logs (if container restarted)
kubectl logs -n gigashop <pod-name> --previous
```

### Database Connection Issues

```bash
# Check if database pod is running
kubectl get pods -n gigashop | grep db

# Test connectivity from API pod
kubectl exec -it -n gigashop <api-pod> -- /bin/sh
# Then: ping catalogdb
```

### Image Pull Errors

```bash
# Check if ACR is attached to AKS
az aks check-acr \
  --name gigashop-aks \
  --resource-group gigashop-rg \
  --acr <acr-name>.azurecr.io
```

## Maintenance

### Update Deployment

```bash
# Update image
kubectl set image deployment/catalog-api \
  catalog-api=<acr>.azurecr.io/catalog-api:v2.0.0 \
  -n gigashop

# Or edit deployment
kubectl edit deployment catalog-api -n gigashop
```

### Rollback Deployment

```bash
# View rollout history
kubectl rollout history deployment/catalog-api -n gigashop

# Rollback to previous version
kubectl rollout undo deployment/catalog-api -n gigashop

# Rollback to specific revision
kubectl rollout undo deployment/catalog-api --to-revision=2 -n gigashop
```

### Restart Deployment

```bash
# Restart all pods in a deployment
kubectl rollout restart deployment/catalog-api -n gigashop
```

## Cleanup

```bash
# Delete all resources in namespace
kubectl delete namespace gigashop

# Or delete specific resources
kubectl delete -k .
```

## Best Practices

1. **Use Namespaces**: Isolate environments (dev, staging, prod)
2. **Set Resource Limits**: Prevent resource exhaustion
3. **Use Health Probes**: Ensure reliability
4. **Enable Autoscaling**: Handle traffic spikes
5. **Use Secrets**: Never hardcode sensitive data
6. **Label Resources**: Organize and filter resources
7. **Use ConfigMaps**: Externalize configuration
8. **Monitor Resources**: Track usage and performance

## Additional Resources

- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [Azure AKS Documentation](https://docs.microsoft.com/en-us/azure/aks/)
- [kubectl Cheat Sheet](https://kubernetes.io/docs/reference/kubectl/cheatsheet/)

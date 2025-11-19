# GigaShop Azure Kubernetes Deployment Guide

This guide provides step-by-step instructions for deploying the GigaShop microservices application to Azure Kubernetes Service (AKS).

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Architecture Overview](#architecture-overview)
3. [Quick Start](#quick-start)
4. [Detailed Setup](#detailed-setup)
5. [Configuration](#configuration)
6. [Monitoring and Troubleshooting](#monitoring-and-troubleshooting)
7. [Scaling](#scaling)
8. [Security Best Practices](#security-best-practices)

## Prerequisites

Before deploying to Azure, ensure you have:

- **Azure Account**: Active Azure subscription ([Create free account](https://azure.microsoft.com/free/))
- **Azure CLI**: Version 2.50.0 or later ([Install guide](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli))
- **kubectl**: Kubernetes command-line tool ([Install guide](https://kubernetes.io/docs/tasks/tools/))
- **Docker**: For building container images ([Install guide](https://docs.docker.com/get-docker/))
- **Git**: For cloning the repository

### Verify Prerequisites

```bash
# Check Azure CLI version
az --version

# Check kubectl version
kubectl version --client

# Check Docker version
docker --version
```

## Architecture Overview

The GigaShop application consists of:

### Microservices
- **Catalog API**: Product catalog management (ASP.NET Core)
- **Basket API**: Shopping cart management (ASP.NET Core)
- **Order API**: Order processing (ASP.NET Core)
- **Discount gRPC**: Discount calculation service (gRPC)
- **API Gateway**: YARP-based reverse proxy

### Databases
- **PostgreSQL**: Catalog and Basket databases
- **SQL Server**: Order database

### Infrastructure
- **Redis**: Distributed caching
- **RabbitMQ**: Message broker for async communication

### Frontend
- **Web**: React/Vite-based web application

## Quick Start

For a rapid deployment, follow these steps:

### 1. Clone and Navigate

```bash
git clone <your-repo-url>
cd gigashop
```

### 2. Configure ACR Name

Edit the following files and replace `gigashopacr` with your unique ACR name:
- `azure/setup-aks.sh`
- `azure/build-and-push.sh`
- `azure/deploy.sh`

### 3. Run Setup Script

```bash
chmod +x azure/*.sh
./azure/setup-aks.sh
```

This script will:
- Create Azure Resource Group
- Create Azure Container Registry (ACR)
- Create AKS cluster with 3 nodes
- Install NGINX Ingress Controller
- Install cert-manager for SSL

### 4. Build and Push Images

```bash
./azure/build-and-push.sh
```

### 5. Deploy Application

```bash
./azure/deploy.sh
```

### 6. Configure DNS

After deployment, get the Ingress IP and configure your DNS:

```bash
kubectl get svc -n ingress-nginx ingress-nginx-controller
```

Point your domains to the external IP:
- `gigashop.yourdomain.com` → Ingress IP
- `api.gigashop.yourdomain.com` → Ingress IP

### 7. Configure SSL Certificates

```bash
# Update email in cert-manager-issuer.yaml
nano azure/cert-manager-issuer.yaml

# Apply cert-manager issuers
kubectl apply -f azure/cert-manager-issuer.yaml
```

## Detailed Setup

### Step 1: Create Azure Resources

The `setup-aks.sh` script creates the following resources:

```bash
# Resource Group
RESOURCE_GROUP="gigashop-rg"
LOCATION="eastus"

# AKS Cluster
AKS_CLUSTER_NAME="gigashop-aks"
NODE_COUNT=3
NODE_SIZE="Standard_D2s_v3"

# Container Registry
ACR_NAME="gigashopacr"  # Must be globally unique
```

**Customization Options:**

```bash
# For production, consider larger nodes
NODE_SIZE="Standard_D4s_v3"

# For high availability
NODE_COUNT=5

# Choose location closest to your users
LOCATION="westeurope"
```

### Step 2: Build Docker Images

The `build-and-push.sh` script builds all microservices:

```bash
# Build with specific tag
./azure/build-and-push.sh v1.0.0

# Build with latest tag (default)
./azure/build-and-push.sh
```

**Manual Build (if needed):**

```bash
# Login to ACR
az acr login --name gigashopacr

# Build individual service
cd src
docker build -t gigashopacr.azurecr.io/catalog-api:v1.0.0 \
    -f Services/Catalog/Catalog.API/Dockerfile .
docker push gigashopacr.azurecr.io/catalog-api:v1.0.0
```

### Step 3: Deploy to Kubernetes

The deployment follows this order:

1. **Namespace**: Creates `gigashop` namespace
2. **Databases**: PostgreSQL and SQL Server
3. **Infrastructure**: Redis and RabbitMQ
4. **Services**: All microservices
5. **Gateway**: API Gateway
6. **Frontend**: Web application
7. **Ingress**: External access configuration

**Manual Deployment:**

```bash
# Apply all manifests
kubectl apply -k k8s/

# Or apply individually
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/databases/
kubectl apply -f k8s/infrastructure/
kubectl apply -f k8s/services/
kubectl apply -f k8s/gateway/
kubectl apply -f k8s/frontend/
kubectl apply -f k8s/ingress/
```

## Configuration

### Environment Variables

Each service can be configured via environment variables in their deployment manifests:

**Catalog API** (`k8s/services/catalog-api-deployment.yaml`):
```yaml
env:
- name: ConnectionStrings__Database
  value: "Host=catalogdb;Port=5432;Database=CatalogDb;Username=postgres;Password=postgres"
```

**Basket API** (`k8s/services/basket-api-deployment.yaml`):
```yaml
env:
- name: ConnectionStrings__Redis
  value: "distributedcache:6379"
- name: GrpcSettings__DiscountUrl
  value: "http://discount-grpc:80"
```

### Secrets Management

**Current Setup** (Development):
Secrets are stored in Kubernetes Secret objects.

**Production Recommendation**:
Use Azure Key Vault with AKS integration:

```bash
# Enable Key Vault integration
az aks enable-addons \
    --addons azure-keyvault-secrets-provider \
    --name gigashop-aks \
    --resource-group gigashop-rg
```

### Storage Configuration

**Storage Classes:**
- `managed-premium`: Azure Premium SSD (default)
- `managed-standard`: Azure Standard HDD (cost-effective)

To use standard storage:

```yaml
spec:
  storageClassName: managed-standard  # Change from managed-premium
```

### Resource Limits

Adjust resource requests/limits based on your needs:

```yaml
resources:
  requests:
    memory: "256Mi"
    cpu: "200m"
  limits:
    memory: "512Mi"
    cpu: "500m"
```

## Monitoring and Troubleshooting

### Check Deployment Status

```bash
# Get all pods
kubectl get pods -n gigashop

# Get all services
kubectl get svc -n gigashop

# Get ingress
kubectl get ingress -n gigashop

# Check pod logs
kubectl logs -n gigashop <pod-name>

# Describe pod for events
kubectl describe pod -n gigashop <pod-name>
```

### Common Issues

**1. Pods in CrashLoopBackOff**

```bash
# Check logs
kubectl logs -n gigashop <pod-name> --previous

# Common causes:
# - Database connection issues
# - Missing environment variables
# - Image pull errors
```

**2. Database Connection Failures**

```bash
# Check database pods
kubectl get pods -n gigashop | grep db

# Test database connectivity
kubectl exec -it -n gigashop <api-pod> -- /bin/sh
# Then try connecting to database
```

**3. Image Pull Errors**

```bash
# Verify ACR integration
az aks check-acr \
    --name gigashop-aks \
    --resource-group gigashop-rg \
    --acr gigashopacr.azurecr.io
```

### Azure Monitor Integration

The AKS cluster is created with Azure Monitor enabled:

```bash
# View in Azure Portal
# Navigate to: AKS Cluster → Monitoring → Insights

# Or use CLI
az monitor metrics list \
    --resource <aks-resource-id> \
    --metric-names "node_cpu_usage_percentage"
```

## Scaling

### Manual Scaling

```bash
# Scale specific deployment
kubectl scale deployment catalog-api -n gigashop --replicas=5

# Scale all services
kubectl scale deployment --all -n gigashop --replicas=3
```

### Horizontal Pod Autoscaling (HPA)

HPA is already configured for all services:

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: catalog-api-hpa
spec:
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

**View HPA status:**

```bash
kubectl get hpa -n gigashop
```

### Cluster Autoscaling

Enable cluster autoscaler:

```bash
az aks update \
    --resource-group gigashop-rg \
    --name gigashop-aks \
    --enable-cluster-autoscaler \
    --min-count 3 \
    --max-count 10
```

## Security Best Practices

### 1. Use Managed Identity

Already configured in `setup-aks.sh`:

```bash
--enable-managed-identity \
--attach-acr $ACR_NAME
```

### 2. Network Policies

Enable network policies to restrict pod-to-pod communication:

```bash
# Already enabled in setup-aks.sh
--network-policy azure
```

### 3. Update Secrets

**Change default passwords:**

```bash
# Update database secrets
kubectl edit secret catalogdb-secret -n gigashop
kubectl edit secret basketdb-secret -n gigashop
kubectl edit secret orderdb-secret -n gigashop
kubectl edit secret rabbitmq-secret -n gigashop
```

### 4. Enable Pod Security Standards

```bash
kubectl label namespace gigashop \
    pod-security.kubernetes.io/enforce=restricted \
    pod-security.kubernetes.io/audit=restricted \
    pod-security.kubernetes.io/warn=restricted
```

### 5. Regular Updates

```bash
# Update AKS cluster
az aks upgrade \
    --resource-group gigashop-rg \
    --name gigashop-aks \
    --kubernetes-version 1.29

# Update node images
az aks nodepool upgrade \
    --resource-group gigashop-rg \
    --cluster-name gigashop-aks \
    --name nodepool1 \
    --node-image-only
```

## Cost Optimization

### 1. Use Azure Spot Instances

```bash
az aks nodepool add \
    --resource-group gigashop-rg \
    --cluster-name gigashop-aks \
    --name spotnodepool \
    --priority Spot \
    --eviction-policy Delete \
    --spot-max-price -1 \
    --node-count 2
```

### 2. Use Standard Storage

Change `storageClassName` to `managed-standard` for non-critical data.

### 3. Right-size Resources

Monitor actual usage and adjust resource requests/limits accordingly.

### 4. Use Azure Dev/Test Pricing

For non-production environments, use Dev/Test subscription for discounts.

## Cleanup

To delete all Azure resources:

```bash
./azure/cleanup.sh
```

Or manually:

```bash
az group delete --name gigashop-rg --yes --no-wait
```

## Support and Troubleshooting

### Useful Commands

```bash
# Get cluster info
kubectl cluster-info

# Get all resources in namespace
kubectl get all -n gigashop

# Port forward to service
kubectl port-forward -n gigashop svc/catalog-api 8080:80

# Execute command in pod
kubectl exec -it -n gigashop <pod-name> -- /bin/bash

# View events
kubectl get events -n gigashop --sort-by='.lastTimestamp'
```

### Azure Support

- [Azure Support Portal](https://portal.azure.com/#blade/Microsoft_Azure_Support/HelpAndSupportBlade)
- [AKS Documentation](https://docs.microsoft.com/en-us/azure/aks/)
- [Azure Status](https://status.azure.com/)

## Next Steps

1. **Set up CI/CD**: Integrate with Azure DevOps or GitHub Actions
2. **Configure Monitoring**: Set up Application Insights
3. **Implement Backup**: Configure Azure Backup for databases
4. **Set up Alerts**: Configure Azure Monitor alerts
5. **Performance Testing**: Load test your application

---

**Last Updated**: 2025-10-13
**Version**: 1.0.0

#!/bin/bash

# Deploy GigaShop to Azure Kubernetes Service
# This script deploys all Kubernetes resources to AKS

set -e

# Configuration
NAMESPACE="gigashop"
ACR_NAME="gigashopacr"  # Change this to match your ACR name

echo "========================================="
echo "Deploying GigaShop to AKS"
echo "========================================="

# Check if kubectl is configured
if ! kubectl cluster-info &> /dev/null; then
    echo "Error: kubectl is not configured or cluster is not accessible"
    exit 1
fi

# Get ACR login server
ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)
echo "Using ACR: $ACR_LOGIN_SERVER"

# Ensure ACR credentials are configured in the cluster
echo "Configuring ACR credentials in Kubernetes..."
kubectl create secret docker-registry acr-secret \
    --docker-server=$ACR_LOGIN_SERVER \
    --docker-username=$(az acr credential show --name $ACR_NAME --query username --output tsv) \
    --docker-password=$(az acr credential show --name $ACR_NAME --query passwords[0].value --output tsv) \
    --namespace=$NAMESPACE \
    --dry-run=client -o yaml | kubectl apply -f -

# Update image references in manifests
echo "Updating image references in Kubernetes manifests..."
cd "$(dirname "$0")/../k8s"

# Use sed to replace <ACR_NAME> placeholder with actual ACR login server
find . -type f -name "*.yaml" -exec sed -i "s|<ACR_NAME>|$ACR_LOGIN_SERVER|g" {} +

# Fix any double .azurecr.io issues that might have been created
find . -type f -name "*.yaml" -exec sed -i "s|\.azurecr\.io\.azurecr\.io|\.azurecr\.io|g" {} +

# Add imagePullSecrets to all deployment files that don't already have them
echo "Adding imagePullSecrets to deployment files..."
find . -name "*deployment.yaml" -exec grep -L "imagePullSecrets" {} \; | while read file; do
    # Add imagePullSecrets after the spec: line in the pod template
    sed -i '/^    spec:$/a\      imagePullSecrets:\n      - name: acr-secret' "$file"
done

# Reduce resource requirements to fit smaller clusters
echo "Reducing resource requirements for smaller clusters..."
find . -name "*deployment.yaml" -exec sed -i 's/cpu: "200m"/cpu: "50m"/g' {} \;
find . -name "*deployment.yaml" -exec sed -i 's/cpu: "250m"/cpu: "100m"/g' {} \;
find . -name "*deployment.yaml" -exec sed -i 's/cpu: "100m"/cpu: "50m"/g' {} \;
find . -name "*deployment.yaml" -exec sed -i 's/cpu: "500m"/cpu: "200m"/g' {} \;
find . -name "*deployment.yaml" -exec sed -i 's/cpu: "1"/cpu: "500m"/g' {} \;

# Reduce memory requirements
find . -name "*deployment.yaml" -exec sed -i 's/memory: "256Mi"/memory: "128Mi"/g' {} \;
find . -name "*deployment.yaml" -exec sed -i 's/memory: "512Mi"/memory: "256Mi"/g' {} \;
find . -name "*deployment.yaml" -exec sed -i 's/memory: "2Gi"/memory: "1Gi"/g' {} \;
find . -name "*deployment.yaml" -exec sed -i 's/memory: "4Gi"/memory: "2Gi"/g' {} \;

# Reduce replica counts to fit smaller clusters
find . -name "*deployment.yaml" -exec sed -i 's/replicas: 2/replicas: 1/g' {} \;

# Fix storage class to use default Azure storage
echo "Fixing storage class references..."
find . -name "*.yaml" -exec sed -i 's/storageClassName: managed-free/storageClassName: managed-csi/g' {} \;
find . -name "*.yaml" -exec sed -i 's/storageClassName: "managed-free"/storageClassName: "managed-csi"/g' {} \;

# Clean up old failed deployments and PVCs
echo "Cleaning up old failed deployments and PVCs..."
kubectl delete deployment --all -n $NAMESPACE --ignore-not-found=true
kubectl delete pod --all -n $NAMESPACE --ignore-not-found=true
kubectl delete pvc --all -n $NAMESPACE --ignore-not-found=true

# Apply Kubernetes manifests
echo "Creating namespace..."
kubectl apply -f namespace.yaml

echo "Deploying databases..."
kubectl apply -f databases/

echo "Deploying infrastructure (Redis, RabbitMQ)..."
kubectl apply -f infrastructure/

echo "Waiting for databases and infrastructure to be ready..."
sleep 30

echo "Deploying microservices..."
kubectl apply -f services/

echo "Deploying API Gateway..."
kubectl apply -f gateway/

echo "Deploying Web Frontend..."
kubectl apply -f frontend/

echo "Deploying Ingress..."
kubectl apply -f ingress/

# Wait for deployments to be ready
echo "Waiting for deployments to be ready..."

# First, check if deployments are even starting
echo "Checking deployment status..."
kubectl get deployments -n $NAMESPACE

# Check for any failed pods
echo "Checking for failed pods..."
kubectl get pods -n $NAMESPACE --field-selector=status.phase!=Running,status.phase!=Succeeded

# Wait for deployments with better error handling
if ! kubectl wait --for=condition=available --timeout=300s \
    deployment --all -n $NAMESPACE; then
    echo "Deployment timeout! Checking pod status..."
    kubectl get pods -n $NAMESPACE
    echo "Checking pod events..."
    kubectl get events -n $NAMESPACE --sort-by='.lastTimestamp'
    echo "Checking deployment status..."
    kubectl describe deployments -n $NAMESPACE
    exit 1
fi

# Get Ingress IP
echo "Getting Ingress external IP..."
INGRESS_IP=""
while [ -z "$INGRESS_IP" ]; do
    echo "Waiting for external IP..."
    INGRESS_IP=$(kubectl get svc -n ingress-nginx ingress-nginx-controller \
        -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
    [ -z "$INGRESS_IP" ] && sleep 10
done

echo "========================================="
echo "Deployment Complete!"
echo "========================================="
echo "Namespace: $NAMESPACE"
echo "Ingress IP: $INGRESS_IP"
echo ""
echo "Configure your DNS:"
echo "  gigashop.yourdomain.com -> $INGRESS_IP"
echo "  api.gigashop.yourdomain.com -> $INGRESS_IP"
echo ""
echo "Check deployment status:"
echo "  kubectl get pods -n $NAMESPACE"
echo "  kubectl get svc -n $NAMESPACE"
echo "  kubectl get ingress -n $NAMESPACE"
echo "========================================="

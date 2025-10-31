#!/bin/bash

# Azure AKS Setup Script for GigaShop
# This script creates and configures an Azure Kubernetes Service cluster

set -e

# Configuration Variables
RESOURCE_GROUP="gigashop-rg"
LOCATION="centralus"
AKS_CLUSTER_NAME="gigashop-aks"
ACR_NAME="gigashopacr" 
NODE_COUNT=1  # Use 1 node for demo (change to 3 for production)
NODE_SIZE="Standard_B2s"  # Use smaller node for demo (change to Standard_D2s_v3 for production)
K8S_VERSION="1.32.7"

echo "========================================="
echo "GigaShop AKS Deployment Setup"
echo "========================================="

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    echo "Error: Azure CLI is not installed. Please install it first."
    echo "Visit: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli"
    exit 1
fi

# Login to Azure
echo "Logging in to Azure..."
az login

# Create Resource Group
echo "Creating resource group: $RESOURCE_GROUP in $LOCATION..."
az group create \
    --name $RESOURCE_GROUP \
    --location $LOCATION

# Create Azure Container Registry
echo "Creating Azure Container Registry: $ACR_NAME..."
az acr create \
    --resource-group $RESOURCE_GROUP \
    --name $ACR_NAME \
    --sku Basic \
    --location $LOCATION

# Enable admin access to ACR (for development, use managed identity in production)
az acr update \
    --name $ACR_NAME \
    --admin-enabled true

# Create AKS Cluster with ACR integration
echo "Creating AKS cluster: $AKS_CLUSTER_NAME..."
az aks create \
    --resource-group $RESOURCE_GROUP \
    --name $AKS_CLUSTER_NAME \
    --node-count $NODE_COUNT \
    --node-vm-size $NODE_SIZE \
    --kubernetes-version $K8S_VERSION \
    --enable-managed-identity \
    --attach-acr $ACR_NAME \
    --generate-ssh-keys \
    --location $LOCATION \
    --network-plugin azure \
    --network-policy azure

# Get AKS credentials
echo "Getting AKS credentials..."
az aks get-credentials \
    --resource-group $RESOURCE_GROUP \
    --name $AKS_CLUSTER_NAME \
    --overwrite-existing

# Install NGINX Ingress Controller
echo "Installing NGINX Ingress Controller..."
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.9.4/deploy/static/provider/cloud/deploy.yaml

# Wait for NGINX Ingress to be ready
echo "Waiting for NGINX Ingress Controller to be ready..."
kubectl wait --namespace ingress-nginx \
    --for=condition=ready pod \
    --selector=app.kubernetes.io/component=controller \
    --timeout=300s

# Install cert-manager for SSL certificates
echo "Installing cert-manager..."
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.2/cert-manager.yaml

# Wait for cert-manager to be ready
echo "Waiting for cert-manager to be ready..."
kubectl wait --namespace cert-manager \
    --for=condition=ready pod \
    --selector=app.kubernetes.io/instance=cert-manager \
    --timeout=300s

# Get ACR login server
ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)

echo "========================================="
echo "Setup Complete!"
echo "========================================="
echo "Resource Group: $RESOURCE_GROUP"
echo "AKS Cluster: $AKS_CLUSTER_NAME"
echo "ACR Name: $ACR_NAME"
echo "ACR Login Server: $ACR_LOGIN_SERVER"
echo ""
echo "Next Steps:"
echo "1. Update k8s manifests with ACR name: $ACR_LOGIN_SERVER"
echo "2. Build and push Docker images: ./build-and-push.sh"
echo "3. Deploy application: kubectl apply -k k8s/"
echo "========================================="

#!/bin/bash

# Build and Push Docker Images to Azure Container Registry
# This script builds all microservices and pushes them to ACR

set -e

# Configuration
ACR_NAME="gigashopacr"
TAG="${1:-latest}"

echo "========================================="
echo "Building and Pushing Docker Images"
echo "========================================="

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    echo "Error: Azure CLI is not installed."
    exit 1
fi

# Login to ACR
echo "Logging in to Azure Container Registry..."
az acr login --name $ACR_NAME

# Get ACR login server
ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)
echo "ACR Login Server: $ACR_LOGIN_SERVER"

# Navigate to src directory
cd "$(dirname "$0")/../src"

# Build and push Catalog API
echo "Building Catalog API..."
docker build -t $ACR_LOGIN_SERVER/catalog-api:$TAG \
    -f Services/Catalog/Catalog.API/Dockerfile .
echo "Pushing Catalog API..."
docker push $ACR_LOGIN_SERVER/catalog-api:$TAG

# Build and push Basket API
echo "Building Basket API..."
docker build -t $ACR_LOGIN_SERVER/basket-api:$TAG \
    -f Services/Basket/Basket.API/Dockerfile .
echo "Pushing Basket API..."
docker push $ACR_LOGIN_SERVER/basket-api:$TAG

# Build and push Order API
echo "Building Order API..."
docker build -t $ACR_LOGIN_SERVER/order-api:$TAG \
    -f Services/Order/Order.API/Dockerfile .
echo "Pushing Order API..."
docker push $ACR_LOGIN_SERVER/order-api:$TAG

# Build and push Discount gRPC
echo "Building Discount gRPC..."
docker build -t $ACR_LOGIN_SERVER/discount-grpc:$TAG \
    -f Services/Discount/Discount.Grpc/Dockerfile .
echo "Pushing Discount gRPC..."
docker push $ACR_LOGIN_SERVER/discount-grpc:$TAG

# Build and push API Gateway
echo "Building API Gateway..."
docker build -t $ACR_LOGIN_SERVER/apigateway:$TAG \
    -f ApiGateways/YarpApiGateway/Dockerfile .
echo "Pushing API Gateway..."
docker push $ACR_LOGIN_SERVER/apigateway:$TAG

# Build and push Web Frontend
echo "Building Web Frontend..."
docker build -t $ACR_LOGIN_SERVER/web:$TAG \
    -f Web/Dockerfile \
    --build-arg VITE_CART_SERVICE_URL=http://api.gigashop.yourdomain.com/basket-service \
    --build-arg VITE_PRODUCT_SERVICE_URL=http://api.gigashop.yourdomain.com/catalog-service \
    --build-arg VITE_USER_SERVICE_URL=http://api.gigashop.yourdomain.com/order-service \
    .
echo "Pushing Web Frontend..."
docker push $ACR_LOGIN_SERVER/web:$TAG

echo "========================================="
echo "All images built and pushed successfully!"
echo "========================================="
echo "Images pushed with tag: $TAG"
echo "ACR: $ACR_LOGIN_SERVER"
echo ""
echo "Next step: Deploy to Kubernetes"
echo "kubectl apply -k k8s/"
echo "========================================="

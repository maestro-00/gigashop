#!/bin/bash

# Cleanup Azure Resources
# WARNING: This will delete all Azure resources created for GigaShop

set -e

RESOURCE_GROUP="gigashop-rg"

echo "========================================="
echo "WARNING: This will delete all resources!"
echo "========================================="
echo "Resource Group: $RESOURCE_GROUP"
echo ""
read -p "Are you sure you want to continue? (yes/no): " confirm

if [ "$confirm" != "yes" ]; then
    echo "Cleanup cancelled."
    exit 0
fi

echo "Deleting resource group: $RESOURCE_GROUP..."
az group delete \
    --name $RESOURCE_GROUP \
    --yes \
    --no-wait

echo "========================================="
echo "Cleanup initiated!"
echo "========================================="
echo "Resource group deletion is in progress."
echo "This may take several minutes to complete."
echo "Check status: az group show --name $RESOURCE_GROUP"
echo "========================================="

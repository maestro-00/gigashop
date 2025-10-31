#!/bin/bash

# Check AKS cluster resources and capacity
echo "========================================="
echo "Checking AKS Cluster Resources"
echo "========================================="

echo "Node information:"
kubectl get nodes -o wide

echo ""
echo "Node resource capacity:"
kubectl describe nodes | grep -A 5 "Capacity:"

echo ""
echo "Current resource usage:"
kubectl top nodes 2>/dev/null || echo "Metrics server not available"

echo ""
echo "Available storage classes:"
kubectl get storageclass

echo ""
echo "Current pods in gigashop namespace:"
kubectl get pods -n gigashop

echo ""
echo "Resource requests and limits:"
kubectl describe nodes | grep -A 10 "Allocated resources:"

echo "========================================="

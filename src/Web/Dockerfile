# Multi-stage build for React frontend
FROM node:18-alpine AS build

# Set working directory
WORKDIR /app

# Copy package files
COPY Web/package*.json ./

# Install dependencies (including dev dependencies for build)
RUN npm ci

# Copy source code
COPY Web/ .

# Set environment variables for build
ARG VITE_CART_SERVICE_URL=http://yarpapigateway:8080/basket-service
ARG VITE_PRODUCT_SERVICE_URL=http://yarpapigateway:8080/catalog-service
ARG VITE_USER_SERVICE_URL=http://yarpapigateway:8080/order-service

# Set environment variables
ENV VITE_CART_SERVICE_URL=$VITE_CART_SERVICE_URL
ENV VITE_PRODUCT_SERVICE_URL=$VITE_PRODUCT_SERVICE_URL
ENV VITE_USER_SERVICE_URL=$VITE_USER_SERVICE_URL

# Build the application
RUN npm run build

# Production stage with nginx
FROM nginx:alpine

# Copy built assets from build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Copy nginx configuration
COPY Web/nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]

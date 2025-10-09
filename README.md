# 🛒 GigaShop

A modern, cloud-native e-commerce platform built with microservices architecture, demonstrating best practices in distributed systems design.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18.3-61DAFB?logo=react)](https://reactjs.org/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)](https://www.docker.com/)

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Development](#development)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

GigaShop is a full-featured e-commerce platform built using microservices architecture. It demonstrates modern software development practices including:

- **Microservices Architecture**: Independent, scalable services
- **Event-Driven Communication**: Using RabbitMQ for async messaging
- **API Gateway Pattern**: YARP-based gateway for unified API access
- **CQRS & Clean Architecture**: Separation of concerns and maintainable code
- **Containerization**: Docker-based deployment
- **Modern Frontend**: React with TypeScript and shadcn/ui components

## 🏗️ Architecture

The application consists of the following microservices:

```
┌─────────────┐
│   Web UI    │ (React + Vite + TypeScript)
└──────┬──────┘
       │
┌──────▼──────────────┐
│  YARP API Gateway   │
└──────┬──────────────┘
       │
       ├─────────────┬─────────────┬─────────────┐
       │             │             │             │
┌──────▼──────┐ ┌───▼────┐ ┌──────▼──────┐ ┌───▼────────┐
│  Catalog    │ │ Basket │ │   Order     │ │  Discount  │
│   Service   │ │ Service│ │   Service   │ │  Service   │
│ (PostgreSQL)│ │(Redis) │ │(SQL Server) │ │   (gRPC)   │
└─────────────┘ └────────┘ └─────────────┘ └────────────┘
       │             │             │
       └─────────────┴─────────────┘
                     │
              ┌──────▼──────┐
              │  RabbitMQ   │
              │(Message Bus)│
              └─────────────┘
```

### Services

- **Catalog Service**: Product catalog management with PostgreSQL
- **Basket Service**: Shopping cart with Redis cache
- **Order Service**: Order processing with SQL Server (CQRS pattern)
- **Discount Service**: Discount calculations via gRPC
- **API Gateway**: YARP reverse proxy for routing and aggregation
- **Web UI**: React-based customer-facing application

## ✨ Features

- 🛍️ Product browsing and search
- 🛒 Shopping cart management
- 💳 Order processing and tracking
- 🎫 Discount and coupon system
- 🔄 Real-time inventory updates
- 📱 Responsive web interface
- 🔐 Secure API gateway
- 📊 Event-driven architecture
- 🐳 Containerized deployment

## 🛠️ Tech Stack

### Backend
- **.NET 8**: Latest LTS version of .NET
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for data access
- **MediatR**: CQRS and mediator pattern
- **Carter**: Minimal API endpoints
- **Mapster**: Object mapping
- **FluentValidation**: Input validation
- **YARP**: Reverse proxy and API gateway
- **gRPC**: High-performance RPC framework

### Frontend
- **React 18**: UI library
- **TypeScript**: Type-safe JavaScript
- **Vite**: Build tool and dev server
- **TailwindCSS**: Utility-first CSS
- **shadcn/ui**: Component library
- **React Router**: Client-side routing
- **TanStack Query**: Data fetching and caching
- **React Hook Form**: Form management
- **Zod**: Schema validation

### Infrastructure
- **Docker & Docker Compose**: Containerization
- **PostgreSQL**: Catalog and Basket databases
- **SQL Server**: Order database
- **Redis**: Distributed cache
- **RabbitMQ**: Message broker

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- **Docker Desktop** (v20.10 or higher)
  - [Download for Windows](https://docs.docker.com/desktop/install/windows-install/)
  - [Download for Mac](https://docs.docker.com/desktop/install/mac-install/)
  - [Download for Linux](https://docs.docker.com/desktop/install/linux-install/)

- **.NET 8 SDK** (for local development)
  - [Download .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)

- **Node.js 18+** (for frontend development)
  - [Download Node.js](https://nodejs.org/)

- **Git**
  - [Download Git](https://git-scm.com/downloads)

### Verify Installation

```bash
# Check Docker
docker --version
docker compose version

# Check .NET
dotnet --version

# Check Node.js
node --version
npm --version
```

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Lukada-taiya/gigashop.git
cd gigashop
```

### 2. Run with Docker Compose (Recommended)

The easiest way to run the entire application:

```bash
cd src
docker compose up -d
```

This will start all services:
- **Web UI**: http://localhost:6005
- **API Gateway**: http://localhost:6004
- **Catalog API**: http://localhost:6000
- **Basket API**: http://localhost:6001
- **Order API**: http://localhost:6003
- **Discount gRPC**: http://localhost:6002
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)

### 3. Verify Services are Running

```bash
# Check all containers are up
docker compose ps

# View logs
docker compose logs -f

# View specific service logs
docker compose logs -f catalog.api
```

### 4. Stop the Application

```bash
# Stop all services
docker compose down

# Stop and remove volumes (clean slate)
docker compose down -v
```

## 📁 Project Structure

```
gigashop/
├── src/
│   ├── ApiGateways/
│   │   └── YarpApiGateway/          # API Gateway service
│   ├── BuildingBlocks/
│   │   ├── BuildingBlocks/          # Shared libraries
│   │   └── BuildingBlocks.Messaging/ # Messaging abstractions
│   ├── Services/
│   │   ├── Basket/
│   │   │   └── Basket.API/          # Basket microservice
│   │   ├── Catalog/
│   │   │   └── Catalog.API/         # Catalog microservice
│   │   ├── Discount/
│   │   │   └── Discount.Grpc/       # Discount gRPC service
│   │   └── Order/
│   │       ├── Order.API/           # Order API
│   │       ├── Order.Application/   # Business logic
│   │       ├── Order.Domain/        # Domain models
│   │       └── Order.Infrastructure/ # Data access
│   ├── Web/                         # React frontend
│   ├── compose.yaml                 # Docker Compose base
│   ├── compose.override.yaml        # Docker Compose overrides
│   └── gigashop.sln                 # Solution file
├── .github/                         # GitHub templates
├── CONTRIBUTING.md                  # Contribution guidelines
├── CODE_OF_CONDUCT.md              # Code of conduct
├── LICENSE                          # MIT License
└── README.md                        # This file
```

## 💻 Development

### Running Services Individually

#### Backend Services

```bash
cd src

# Run Catalog API
dotnet run --project Services/Catalog/Catalog.API

# Run Basket API
dotnet run --project Services/Basket/Basket.API

# Run Order API
dotnet run --project Services/Order/Order.API

# Run Discount gRPC
dotnet run --project Services/Discount/Discount.Grpc

# Run API Gateway
dotnet run --project ApiGateways/YarpApiGateway
```

#### Frontend

```bash
cd src/Web

# Install dependencies
npm install

# Run development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Lint code
npm run lint
```

### Database Migrations

```bash
# Add a new migration (Order service example)
cd src/Services/Order/Order.API
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Building Docker Images

```bash
cd src

# Build all images
docker compose build

# Build specific service
docker compose build catalog.api

# Build without cache
docker compose build --no-cache
```

### Environment Variables

Create a `.env` file in the `src/` directory for local configuration:

```env
# Database Connection Strings
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=CatalogDb

# SQL Server
SA_PASSWORD=YourStrong@Passw0rd

# Redis
REDIS_PASSWORD=

# RabbitMQ
RABBITMQ_DEFAULT_USER=guest
RABBITMQ_DEFAULT_PASS=guest

# API URLs
VITE_CART_SERVICE_URL=http://localhost:6004/basket-service
VITE_PRODUCT_SERVICE_URL=http://localhost:6004/catalog-service
VITE_USER_SERVICE_URL=http://localhost:6004/order-service
```

## 📚 API Documentation

### Catalog Service
- `GET /products` - Get all products
- `GET /products/{id}` - Get product by ID
- `GET /products/category/{category}` - Get products by category
- `POST /products` - Create new product
- `PUT /products` - Update product
- `DELETE /products/{id}` - Delete product

### Basket Service
- `GET /basket/{userName}` - Get user's basket
- `POST /basket` - Store/update basket
- `DELETE /basket/{userName}` - Delete basket
- `POST /basket/checkout` - Checkout basket

### Order Service
- `GET /orders` - Get all orders
- `GET /orders/{orderName}` - Get orders by name
- `GET /orders/customer/{customerId}` - Get customer orders
- `POST /orders` - Create order
- `PUT /orders` - Update order
- `DELETE /orders/{id}` - Delete order

### Discount Service (gRPC)
- `GetDiscount` - Get discount for a product
- `CreateDiscount` - Create new discount
- `UpdateDiscount` - Update existing discount
- `DeleteDiscount` - Delete discount

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run specific test project
dotnet test src/Services/Catalog/Catalog.Tests
```

## 🐛 Troubleshooting

### Common Issues

**Port conflicts:**
```bash
# Check what's using a port
lsof -i :6000  # macOS/Linux
netstat -ano | findstr :6000  # Windows

# Change ports in compose.override.yaml
```

**Database connection issues:**
```bash
# Reset databases
docker compose down -v
docker compose up -d
```

**Frontend build errors:**
```bash
cd src/Web
rm -rf node_modules package-lock.json
npm install
```

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details on:

- Code of Conduct
- Development process
- Submitting pull requests
- Coding standards
- Testing requirements

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Adam Tungtaiya Lukman** - *Initial work* - [@Lukada-taiya](https://github.com/Lukada-taiya)

## 🙏 Acknowledgments

- Built with modern .NET and React ecosystems
- Inspired by microservices best practices
- Community-driven development

## 📞 Support

- 📧 Email: [Create an issue](https://github.com/Lukada-taiya/gigashop/issues)
- 💬 Discussions: [GitHub Discussions](https://github.com/Lukada-taiya/gigashop/discussions)
- 🐛 Bug Reports: [Issue Tracker](https://github.com/Lukada-taiya/gigashop/issues)

---

**Star ⭐ this repository if you find it helpful!**
# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project setup with microservices architecture
- Catalog Service with PostgreSQL database
- Basket Service with Redis cache
- Order Service with SQL Server and CQRS pattern
- Discount Service using gRPC
- YARP API Gateway for routing
- React frontend with TypeScript and shadcn/ui
- Docker Compose configuration for local development
- Comprehensive documentation (README, CONTRIBUTING, CODE_OF_CONDUCT)
- Issue and PR templates
- Security policy

### Changed
- N/A

### Deprecated
- N/A

### Removed
- N/A

### Fixed
- N/A

### Security
- N/A

## [0.1.0] - 2025-10-09

### Added
- Initial release
- Basic e-commerce functionality
- Microservices architecture
- Event-driven communication with RabbitMQ
- Docker containerization

---

## How to Update This Changelog

When making changes, add entries under the `[Unreleased]` section in the appropriate category:

- **Added** for new features
- **Changed** for changes in existing functionality
- **Deprecated** for soon-to-be removed features
- **Removed** for now removed features
- **Fixed** for any bug fixes
- **Security** for vulnerability fixes

When releasing a new version, move items from `[Unreleased]` to a new version section with the date.

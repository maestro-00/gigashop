# Contributing to GigaShop

First off, thank you for considering contributing to GigaShop! It's people like you that make GigaShop such a great tool.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)

## Code of Conduct

This project and everyone participating in it is governed by our [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to the project maintainers.

## Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/gigashop.git
   cd gigashop
   ```
3. **Add upstream remote**:
   ```bash
   git remote add upstream https://github.com/Lukada-taiya/gigashop.git
   ```
4. **Create a branch** for your changes:
   ```bash
   git checkout -b feature/your-feature-name
   ```

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues to avoid duplicates. When creating a bug report, include as many details as possible:

- **Use a clear and descriptive title**
- **Describe the exact steps to reproduce the problem**
- **Provide specific examples** to demonstrate the steps
- **Describe the behavior you observed** and what you expected
- **Include screenshots or animated GIFs** if applicable
- **Include your environment details** (OS, .NET version, Docker version, etc.)

Use the [Bug Report template](.github/ISSUE_TEMPLATE/bug_report.md) when creating an issue.

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion:

- **Use a clear and descriptive title**
- **Provide a detailed description** of the suggested enhancement
- **Explain why this enhancement would be useful**
- **List any alternative solutions** you've considered

Use the [Feature Request template](.github/ISSUE_TEMPLATE/feature_request.md) when creating an issue.

### Your First Code Contribution

Unsure where to begin? Look for issues labeled:

- `good first issue` - Simple issues perfect for newcomers
- `help wanted` - Issues where we need community help
- `documentation` - Documentation improvements

### Pull Requests

1. Follow the [Development Workflow](#development-workflow)
2. Follow the [Coding Standards](#coding-standards)
3. Include tests for your changes
4. Update documentation as needed
5. Follow the [Pull Request Process](#pull-request-process)

## Development Workflow

### Setting Up Your Development Environment

1. **Install prerequisites** (see [README.md](README.md#prerequisites))
2. **Start infrastructure services**:
   ```bash
   cd src
   docker compose up -d catalogdb basketdb orderdb distributedcache messagebroker
   ```
3. **Run the service you're working on** locally
4. **Make your changes**
5. **Test your changes**

### Backend Development (.NET)

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Run a specific service
dotnet run --project src/Services/Catalog/Catalog.API
```

### Frontend Development (React)

```bash
cd src/Web

# Install dependencies
npm install

# Start dev server
npm run dev

# Run linter
npm run lint

# Build for production
npm run build
```

### Working with Docker

```bash
# Build all services
docker compose build

# Build specific service
docker compose build catalog.api

# View logs
docker compose logs -f catalog.api

# Restart a service
docker compose restart catalog.api
```

## Coding Standards

### C# / .NET Guidelines

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use **meaningful names** for variables, methods, and classes
- Keep methods **small and focused** (Single Responsibility Principle)
- Use **async/await** for I/O operations
- Add **XML documentation** for public APIs
- Use **dependency injection** for loose coupling
- Follow **SOLID principles**

**Example:**

```csharp
/// <summary>
/// Retrieves a product by its unique identifier.
/// </summary>
/// <param name="id">The product identifier.</param>
/// <returns>The product if found, null otherwise.</returns>
public async Task<Product?> GetProductByIdAsync(Guid id)
{
    return await _context.Products
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);
}
```

### TypeScript / React Guidelines

- Follow [Airbnb JavaScript Style Guide](https://github.com/airbnb/javascript)
- Use **TypeScript** for type safety
- Use **functional components** with hooks
- Keep components **small and reusable**
- Use **meaningful prop names**
- Add **JSDoc comments** for complex functions
- Use **custom hooks** for reusable logic

**Example:**

```typescript
interface ProductCardProps {
  product: Product;
  onAddToCart: (productId: string) => void;
}

/**
 * Displays a product card with image, title, price, and add to cart button.
 */
export const ProductCard: React.FC<ProductCardProps> = ({ product, onAddToCart }) => {
  return (
    <Card>
      <CardContent>
        <img src={product.imageUrl} alt={product.name} />
        <h3>{product.name}</h3>
        <p>${product.price}</p>
        <Button onClick={() => onAddToCart(product.id)}>
          Add to Cart
        </Button>
      </CardContent>
    </Card>
  );
};
```

### General Guidelines

- **Write self-documenting code** with clear names
- **Add comments** only when necessary to explain "why", not "what"
- **Keep files focused** - one class/component per file
- **Avoid magic numbers** - use named constants
- **Handle errors gracefully** with proper error messages
- **Don't commit commented-out code** - use version control instead
- **Keep dependencies up to date**

## Commit Guidelines

We follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation only changes
- **style**: Code style changes (formatting, missing semi-colons, etc.)
- **refactor**: Code change that neither fixes a bug nor adds a feature
- **perf**: Performance improvements
- **test**: Adding or updating tests
- **chore**: Changes to build process or auxiliary tools
- **ci**: CI/CD configuration changes

### Examples

```bash
feat(catalog): add product search functionality

Implement full-text search for products using PostgreSQL.
Includes filtering by category and price range.

Closes #123

---

fix(basket): resolve race condition in cart updates

Add optimistic locking to prevent concurrent update issues.

Fixes #456

---

docs(readme): update setup instructions for Windows

Add specific steps for Windows users including WSL2 setup.
```

### Commit Best Practices

- Use the **imperative mood** ("Add feature" not "Added feature")
- Keep the **subject line under 50 characters**
- Capitalize the subject line
- Don't end the subject line with a period
- Separate subject from body with a blank line
- Wrap the body at 72 characters
- Use the body to explain **what and why**, not how

## Pull Request Process

### Before Submitting

1. ✅ **Update your branch** with the latest upstream changes:
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. ✅ **Run all tests** and ensure they pass:
   ```bash
   dotnet test
   cd src/Web && npm test
   ```

3. ✅ **Run linters**:
   ```bash
   dotnet format
   cd src/Web && npm run lint
   ```

4. ✅ **Update documentation** if needed

5. ✅ **Add tests** for new features

### Submitting the Pull Request

1. **Push your branch** to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```

2. **Create a Pull Request** on GitHub

3. **Fill out the PR template** completely

4. **Link related issues** using keywords (Closes #123, Fixes #456)

5. **Request review** from maintainers

### PR Title Format

Follow the same format as commit messages:

```
feat(catalog): add product search functionality
fix(basket): resolve race condition in cart updates
docs(readme): update setup instructions
```

### PR Description Template

```markdown
## Description
Brief description of what this PR does.

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## Related Issues
Closes #123
Fixes #456

## How Has This Been Tested?
Describe the tests you ran and how to reproduce them.

## Checklist
- [ ] My code follows the style guidelines of this project
- [ ] I have performed a self-review of my own code
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have made corresponding changes to the documentation
- [ ] My changes generate no new warnings
- [ ] I have added tests that prove my fix is effective or that my feature works
- [ ] New and existing unit tests pass locally with my changes
- [ ] Any dependent changes have been merged and published
```

### Review Process

- Maintainers will review your PR within **3-5 business days**
- Address any **requested changes** promptly
- Keep the PR **focused** - one feature/fix per PR
- Be **responsive** to feedback and questions
- Once approved, a maintainer will **merge** your PR

## Testing Guidelines

### Backend Testing (.NET)

We use **xUnit** for unit testing and integration testing.

```csharp
public class ProductServiceTests
{
    [Fact]
    public async Task GetProductById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(new Product { Id = productId, Name = "Test Product" });
        
        var service = new ProductService(mockRepo.Object);
        
        // Act
        var result = await service.GetProductByIdAsync(productId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }
}
```

### Frontend Testing (React)

We use **Vitest** and **React Testing Library**.

```typescript
import { render, screen, fireEvent } from '@testing-library/react';
import { ProductCard } from './ProductCard';

describe('ProductCard', () => {
  it('calls onAddToCart when button is clicked', () => {
    const mockProduct = { id: '1', name: 'Test Product', price: 99.99 };
    const mockAddToCart = vi.fn();
    
    render(<ProductCard product={mockProduct} onAddToCart={mockAddToCart} />);
    
    const button = screen.getByText('Add to Cart');
    fireEvent.click(button);
    
    expect(mockAddToCart).toHaveBeenCalledWith('1');
  });
});
```

### Test Coverage

- Aim for **at least 80% code coverage**
- Focus on **critical paths** and **edge cases**
- Write **integration tests** for API endpoints
- Write **unit tests** for business logic
- Write **E2E tests** for critical user flows

## Documentation

### Code Documentation

- Add **XML comments** for public C# APIs
- Add **JSDoc comments** for complex TypeScript functions
- Keep comments **up to date** with code changes
- Document **non-obvious decisions** in comments

### README and Guides

- Update **README.md** if you change setup process
- Add **examples** for new features
- Update **API documentation** for endpoint changes
- Create **migration guides** for breaking changes

### Architecture Decision Records (ADRs)

For significant architectural decisions, create an ADR in `docs/adr/`:

```markdown
# ADR-001: Use CQRS Pattern for Order Service

## Status
Accepted

## Context
The order service needs to handle high read and write loads with different optimization strategies.

## Decision
Implement CQRS pattern with separate read and write models.

## Consequences
- Improved scalability
- Better performance optimization
- Increased complexity
- Eventual consistency challenges
```

## Questions?

Feel free to:
- 💬 Start a [GitHub Discussion](https://github.com/Lukada-taiya/gigashop/discussions)
- 🐛 Open an [Issue](https://github.com/Lukada-taiya/gigashop/issues)
- 📧 Contact the maintainers

---

Thank you for contributing to GigaShop! 🎉

import { Header } from '@/components/shop/Header';
import { mockProducts } from '@/data/products';
import { Badge } from '@/components/ui/badge';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { ArrowRight } from 'lucide-react';
import { useMemo } from 'react';
import { useNavigate } from 'react-router-dom';

const Categories = () => {
  const navigate = useNavigate();
  
  const categoryData = useMemo(() => {
    const categoryMap = new Map();
    
    const getCategoryDescription = (categoryName: string) => {
      const descriptions: Record<string, string> = {
        'Electronics': 'Cutting-edge technology and innovative devices for modern living',
        'Clothing': 'Fashion-forward apparel and accessories for every style',
        'Home & Garden': 'Transform your living space with our curated home essentials',
        'Sports': 'Premium sports equipment and gear for active lifestyles',
        'Books': 'Expand your knowledge with our diverse collection of literature',
        'Beauty': 'Premium beauty products and skincare essentials'
      };
      return descriptions[categoryName] || 'Discover amazing products in this category';
    };
    mockProducts.forEach(product => {
      if (!categoryMap.has(product.category)) {
        categoryMap.set(product.category, {
          name: product.category,
          products: [],
          totalProducts: 0,
          priceRange: { min: Infinity, max: 0 }
        });
      }
      
      const category = categoryMap.get(product.category);
      category.products.push(product);
      category.totalProducts++;
      category.priceRange.min = Math.min(category.priceRange.min, product.price);
      category.priceRange.max = Math.max(category.priceRange.max, product.price);
    });

    return Array.from(categoryMap.values()).map(category => ({
      ...category,
      featuredImage: category.products[0]?.images[0] || '/placeholder.svg',
      description: getCategoryDescription(category.name)
    }));
  }, []);


  const handleViewCategory = (categoryName: string) => {
    navigate(`/products?category=${encodeURIComponent(categoryName)}`);
  };

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      <main className="container mx-auto px-4 py-8">
        {/* Page Header */}
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold mb-4 bg-gradient-shop bg-clip-text text-transparent">
            Product Categories
          </h1>
          <p className="text-muted-foreground text-lg max-w-2xl mx-auto">
            Explore our carefully organized product categories to find exactly what you're looking for.
          </p>
        </div>

        {/* Stats Bar */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-12">
          <div className="text-center p-4 bg-card rounded-lg border">
            <div className="text-2xl font-bold text-shop-accent">{categoryData.length}</div>
            <div className="text-sm text-muted-foreground">Categories</div>
          </div>
          <div className="text-center p-4 bg-card rounded-lg border">
            <div className="text-2xl font-bold text-shop-accent">{mockProducts.length}</div>
            <div className="text-sm text-muted-foreground">Total Products</div>
          </div>
          <div className="text-center p-4 bg-card rounded-lg border">
            <div className="text-2xl font-bold text-shop-accent">4.8</div>
            <div className="text-sm text-muted-foreground">Average Rating</div>
          </div>
          <div className="text-center p-4 bg-card rounded-lg border">
            <div className="text-2xl font-bold text-shop-accent">24/7</div>
            <div className="text-sm text-muted-foreground">Support</div>
          </div>
        </div>

        {/* Categories Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {categoryData.map((category) => (
            <Card 
              key={category.name} 
              className="group hover:shadow-lg transition-all duration-300 hover:-translate-y-1 cursor-pointer"
              onClick={() => handleViewCategory(category.name)}
            >
              <div className="aspect-video relative overflow-hidden rounded-t-lg">
                <img
                  src={category.featuredImage}
                  alt={category.name}
                  className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                />
                <div className="absolute top-4 right-4">
                  <Badge variant="secondary" className="bg-background/80 backdrop-blur-sm">
                    {category.totalProducts} items
                  </Badge>
                </div>
              </div>
              
              <CardHeader>
                <CardTitle className="text-xl group-hover:text-shop-accent transition-colors">
                  {category.name}
                </CardTitle>
                <CardDescription>
                  {category.description}
                </CardDescription>
              </CardHeader>
              
              <CardContent>
                <div className="flex items-center justify-between mb-4">
                  <div className="text-sm text-muted-foreground">
                    Price range: ${category.priceRange.min} - ${category.priceRange.max}
                  </div>
                </div>
                
                <Button 
                  variant="outline" 
                  className="w-full group-hover:bg-shop-accent group-hover:text-shop-accent-foreground transition-colors"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleViewCategory(category.name);
                  }}
                >
                  Browse {category.name}
                  <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform" />
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Featured Section */}
        <div className="mt-16 text-center">
          <h2 className="text-2xl font-bold mb-4">Can't find what you're looking for?</h2>
          <p className="text-muted-foreground mb-6">
            Contact our support team and we'll help you find the perfect product.
          </p>
          <Button variant="outline">
            Contact Support
          </Button>
        </div>
      </main>
    </div>
  );
};

export default Categories;
import { Header } from '@/components/shop/Header';
import { ProductCard } from '@/components/shop/ProductCard';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge'; 
import { ArrowRight, ShoppingBag, Star, Users } from 'lucide-react';
import heroImage from '@/assets/hero-banner.jpg';
import { useEffect, useState } from 'react';
import { Product } from '@/types/product';
import { useApi } from '@/hooks/use-api';
import { useNavigate } from 'react-router-dom';

const Index = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const {request, loading} = useApi('product-service');
  const noOfProducts = 100;
  const [featuredProducts, setFeaturedProducts] = useState<Product[]>([]);
  const categories = ['Electronics', 'Accessories', 'Bags', 'Home', 'Wearable','Furniture'];
  const navigate = useNavigate();

  useEffect(() => {
    (async () => {
      const data = await request<Product[]>({
        url: `/products?pageSize=${noOfProducts}`,
        method: "GET",
      });
      if (data) {setProducts(data.products); setFeaturedProducts(data.products.slice(0,6)); }
    })();
  },[]);

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      {/* Hero Section */}
      <section className="relative h-[600px] flex items-center justify-center overflow-hidden">
        <div
          className="absolute inset-0 bg-cover bg-center"
          style={{ backgroundImage: `url(${heroImage})` }}
        />
        <div className="absolute inset-0 bg-gradient-to-r from-black/60 to-black/20" />
        <div className="relative z-10 text-center text-white max-w-4xl mx-auto px-4">
          <h1 className="text-5xl md:text-7xl font-bold mb-6">
            Premium
            <span className="block bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
              Collection
            </span>
          </h1>
          <p className="text-xl md:text-2xl mb-8 text-gray-200">
            Discover luxury products crafted for the modern lifestyle
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Button size="lg" className="bg-shop-accent hover:bg-shop-accent-hover text-shop-accent-foreground" onClick={() => navigate('/products')}>
              <ShoppingBag className="mr-2 h-5 w-5" />
              Shop Now
            </Button>
            <Button size="lg" variant="outline" className="border-white hover:border-slate-200 hover:bg-slate-200 text-primary hover:text-shop-accent transition-colors" onClick={() => navigate('/categories')}>
              Explore Categories
              <ArrowRight className="ml-2 h-5 w-5" />
            </Button>
          </div>
        </div>
      </section>

      {/* Stats Section */}
      <section className="py-16 bg-muted/50">
        <div className="container mx-auto px-4">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 text-center">
            <div className="space-y-2">
              <div className="flex items-center justify-center mb-4">
                <div className="p-3 rounded-full bg-shop-accent/10">
                  <ShoppingBag className="h-8 w-8 text-shop-accent" />
                </div>
              </div>
              <h3 className="text-3xl font-bold">10,000+</h3>
              <p className="text-muted-foreground">Premium Products</p>
            </div>
            <div className="space-y-2">
              <div className="flex items-center justify-center mb-4">
                <div className="p-3 rounded-full bg-shop-accent/10">
                  <Users className="h-8 w-8 text-shop-accent" />
                </div>
              </div>
              <h3 className="text-3xl font-bold">50,000+</h3>
              <p className="text-muted-foreground">Happy Customers</p>
            </div>
            <div className="space-y-2">
              <div className="flex items-center justify-center mb-4">
                <div className="p-3 rounded-full bg-shop-accent/10">
                  <Star className="h-8 w-8 text-shop-accent" />
                </div>
              </div>
              <h3 className="text-3xl font-bold">4.9/5</h3>
              <p className="text-muted-foreground">Customer Rating</p>
            </div>
          </div>
        </div>
      </section>

      {/* Categories Section */}
      <section className="py-16">
        <div className="container mx-auto px-4">
          <div className="text-center mb-12">
            <h2 className="text-4xl font-bold mb-4">Shop by Category</h2>
            <p className="text-xl text-muted-foreground">
              Find exactly what you're looking for
            </p>
          </div>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
            {categories.map((category) => (
              <div
                key={category}
                className="group relative overflow-hidden rounded-lg bg-muted hover:bg-muted/80 p-8 text-center cursor-pointer transition-all duration-300 hover:scale-105"
                onClick={() => navigate(`/products?category=${encodeURIComponent(category)}`)}
              >
                <h3 className="text-xl font-semibold mb-2 group-hover:text-shop-accent transition-colors">
                  {category}
                </h3>
                <p className="text-muted-foreground">
                  {products.filter(p => p.category.includes(category)).length} items
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Featured Products */}
      <section className="py-16 bg-muted/50">
        <div className="container mx-auto px-4">
          <div className="text-center mb-12">
            <h2 className="text-4xl font-bold mb-4">Featured Products</h2>
            <p className="text-xl text-muted-foreground">
              Handpicked favorites from our collection
            </p>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {featuredProducts.map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
          <div className="text-center mt-12">
            <Button size="lg" variant="outline" onClick={() => navigate('/products')}>
              View All Products
              <ArrowRight className="ml-2 h-4 w-4" />
            </Button>
          </div>
        </div>
      </section>

      {/* Newsletter */}
      <section className="py-16">
        <div className="container mx-auto px-4">
          <div className="max-w-2xl mx-auto text-center">
            <h2 className="text-3xl font-bold mb-4">Stay Updated</h2>
            <p className="text-muted-foreground mb-8">
              Subscribe to our newsletter for exclusive offers and new arrivals
            </p>
            <div className="flex gap-4 max-w-md mx-auto">
              <input
                type="email"
                placeholder="Enter your email"
                className="flex-1 px-4 py-2 border border-input rounded-md focus:ring-2 focus:ring-shop-accent focus:border-transparent"
              />
              <Button className="bg-shop-accent hover:bg-shop-accent-hover text-shop-accent-foreground">
                Subscribe
              </Button>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Index;

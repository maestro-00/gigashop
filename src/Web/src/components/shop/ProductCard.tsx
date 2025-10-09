import { Product } from '@/types/product';
import { Card, CardContent } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Star, Heart } from 'lucide-react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

interface ProductCardProps {
  product: Product;
}

export const ProductCard = ({ product }: ProductCardProps) => {
  const [isWishListed, setIsWishListed] = useState(false);
  const navigate = useNavigate();

  const discountPercentage = product.originalPrice 
    ? Math.round(((product.originalPrice - product.price) / product.originalPrice) * 100)
    : 0;

  const handleProductClick = () => {
    navigate(`/product/${product.id}`);
  };

  return (
    <Card className="group overflow-hidden shadow-product hover:shadow-card transition-all duration-300 cursor-pointer">
      <div className="relative overflow-hidden" onClick={handleProductClick}>
        <img
          src={product.images?.[0] || '/placeholder.svg'}
          alt={product.name}
          className="w-full h-64 object-cover group-hover:scale-105 transition-transform duration-300"
          onError={(e) => {
            const target = e.target as HTMLImageElement;
            target.src = '/placeholder.svg';
          }}
        />
        {discountPercentage > 0 && (
          <Badge className="absolute top-3 left-3 bg-sale text-sale-foreground">
            -{discountPercentage}%
          </Badge>
        )}
        <Button
          variant="ghost"
          size="icon"
          className="absolute top-3 right-3 h-8 w-8 bg-background/80 hover:bg-background"
          onClick={(e) => {
            e.stopPropagation();
            setIsWishListed(!isWishListed);
          }}
        >
          <Heart className={`h-4 w-4 ${isWishListed ? 'fill-current text-red-500' : ''}`} />
        </Button>
      </div>
      
      <CardContent className="p-4" onClick={handleProductClick}>
        <div className="space-y-2">
          <h3 className="font-semibold text-lg line-clamp-2 group-hover:text-shop-accent transition-colors">
            {product.name}
          </h3>
          
          <div className="flex items-center space-x-1">
            <div className="flex items-center">
              {[...Array(5)].map((_, i) => (
                <Star
                  key={i}
                  className={`h-4 w-4 ${
                    i < Math.floor(product.rating)
                      ? 'fill-yellow-400 text-yellow-400'
                      : 'text-gray-300'
                  }`}
                />
              ))}
            </div>
            <span className="text-sm text-muted-foreground">
              ({product.reviewCount})
            </span>
          </div>

          <div className="flex items-center space-x-2">
            <span className="text-xl font-bold text-price">
              ${product.price}
            </span>
            {product.originalPrice && (
              <span className="text-sm text-muted-foreground line-through">
                ${product.originalPrice}
              </span>
            )}
          </div>

          <p className="text-sm text-muted-foreground line-clamp-2">
            {product.description}
          </p>

          <div className="flex flex-wrap gap-1 mt-2">
            {product.colors.slice(0, 3).map((color) => (
              <div
                key={color.name}
                className="w-4 h-4 rounded-full border border-gray-300"
                style={{ backgroundColor: color.value }}
                title={color.name}
              />
            ))}
            {product.colors.length > 3 && (
              <span className="text-xs text-muted-foreground">
                +{product.colors.length - 3} more
              </span>
            )}
          </div>
        </div>
      </CardContent>
    </Card>
  );
};
import { Product } from '@/types/product';

export const mockProducts: Product[] = [
  {
    id: '1',
    name: 'Premium Wireless Headphones',
    description: 'High-quality wireless headphones with noise cancellation and premium sound quality. Perfect for music lovers and professionals.',
    price: 299.99,
    originalPrice: 399.99,
    images: [
      'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1484704849700-f032a568e944?w=800&h=600&fit=crop'
    ],
    category: 'Electronics',
    colors: [
      { name: 'Black', value: '#000000' },
      { name: 'White', value: '#FFFFFF' },
      { name: 'Silver', value: '#C0C0C0' }
    ],
    sizes: ['One Size'],
    inStock: true,
    rating: 4.8,
    reviewCount: 124
  },
  {
    id: '2',
    name: 'Luxury Watch Collection',
    description: 'Elegant timepiece crafted with precision. Features premium materials and sophisticated design for the modern professional.',
    price: 799.99,
    images: [
      'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1606760227091-3dd870d97f1d?w=800&h=600&fit=crop'
    ],
    category: 'Accessories',
    colors: [
      { name: 'Gold', value: '#FFD700' },
      { name: 'Silver', value: '#C0C0C0' },
      { name: 'Black', value: '#000000' }
    ],
    sizes: ['38mm', '42mm', '45mm'],
    inStock: true,
    rating: 4.9,
    reviewCount: 89
  },
  {
    id: '3',
    name: 'Designer Sunglasses',
    description: 'Stylish sunglasses with UV protection and premium lens technology. Perfect blend of fashion and functionality.',
    price: 199.99,
    originalPrice: 299.99,
    images: [
      'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1511499767150-a48a237f0083?w=800&h=600&fit=crop'
    ],
    category: 'Accessories',
    colors: [
      { name: 'Black', value: '#000000' },
      { name: 'Tortoiseshell', value: '#8B4513' },
      { name: 'Blue', value: '#0066CC' }
    ],
    sizes: ['One Size'],
    inStock: true,
    rating: 4.6,
    reviewCount: 203
  },
  {
    id: '4',
    name: 'Premium Leather Backpack',
    description: 'Handcrafted leather backpack designed for professionals. Spacious compartments and durable construction.',
    price: 349.99,
    images: [
      'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1622560480605-d83c853bc5c3?w=800&h=600&fit=crop'
    ],
    category: 'Bags',
    colors: [
      { name: 'Brown', value: '#8B4513' },
      { name: 'Black', value: '#000000' },
      { name: 'Tan', value: '#D2B48C' }
    ],
    sizes: ['One Size'],
    inStock: true,
    rating: 4.7,
    reviewCount: 156
  },
  {
    id: '5',
    name: 'Wireless Charging Pad',
    description: 'Fast wireless charging pad compatible with all Qi-enabled devices. Sleek design with LED indicators.',
    price: 79.99,
    images: [
      'https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=800&h=600&fit=crop'
    ],
    category: 'Electronics',
    colors: [
      { name: 'White', value: '#FFFFFF' },
      { name: 'Black', value: '#000000' }
    ],
    sizes: ['One Size'],
    inStock: true,
    rating: 4.5,
    reviewCount: 312
  },
  {
    id: '6',
    name: 'Premium Coffee Mug',
    description: 'Insulated coffee mug that keeps your drinks hot for hours. Perfect for coffee enthusiasts and busy professionals.',
    price: 39.99,
    images: [
      'https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=800&h=600&fit=crop'
    ],
    category: 'Home',
    colors: [
      { name: 'Matte Black', value: '#2C2C2C' },
      { name: 'White', value: '#FFFFFF' },
      { name: 'Steel Blue', value: '#4682B4' }
    ],
    sizes: ['12oz', '16oz'],
    inStock: true,
    rating: 4.4,
    reviewCount: 87
  }
];
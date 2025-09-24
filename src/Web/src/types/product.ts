export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  originalPrice?: number;
  images: string[];
  category: string;
  colors: Color[];
  sizes: string[];
  inStock: boolean;
  rating: number;
  reviewCount: number;
}

export interface Color {
  name: string;
  value: string;
}

export interface CartItem {
  product: Product;
  quantity: number;
  selectedColor: Color;
  selectedSize: string;
}

export interface Order {
  id: string;
  items: CartItem[];
  total: number;
  status: 'pending' | 'processing' | 'shipped' | 'delivered' | 'cancelled';
  createdAt: string;
  shippingAddress: Address;
  billingAddress: Address;
}

export interface Address {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
}
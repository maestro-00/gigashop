import { useState, useEffect } from 'react';
import { CartItem, Product, Color } from '@/types/product';

export const useCart = () => {
  const [cartItems, setCartItems] = useState<CartItem[]>([]);

  useEffect(() => {
    const savedCart = localStorage.getItem('cart');
    if (savedCart) {
      setCartItems(JSON.parse(savedCart));
    }
  }, []);

  useEffect(() => {
    localStorage.setItem('cart', JSON.stringify(cartItems));
  }, [cartItems]);

  const addToCart = (product: Product, selectedColor: Color, selectedSize: string, quantity: number = 1) => {
    setCartItems(prevItems => {
      const existingItem = prevItems.find(
        item => 
          item.product.id === product.id && 
          item.selectedColor.value === selectedColor.value && 
          item.selectedSize === selectedSize
      );

      if (existingItem) {
        return prevItems.map(item =>
          item === existingItem
            ? { ...item, quantity: item.quantity + quantity }
            : item
        );
      }

      return [...prevItems, { product, selectedColor, selectedSize, quantity }];
    });
  };

  const removeFromCart = (productId: string, colorValue: string, size: string) => {
    setCartItems(prevItems =>
      prevItems.filter(
        item =>
          !(item.product.id === productId && 
            item.selectedColor.value === colorValue && 
            item.selectedSize === size)
      )
    );
  };

  const updateQuantity = (productId: string, colorValue: string, size: string, quantity: number) => {
    if (quantity <= 0) {
      removeFromCart(productId, colorValue, size);
      return;
    }

    setCartItems(prevItems =>
      prevItems.map(item =>
        item.product.id === productId && 
        item.selectedColor.value === colorValue && 
        item.selectedSize === size
          ? { ...item, quantity }
          : item
      )
    );
  };

  const clearCart = () => {
    setCartItems([]);
  };

  const getCartTotal = () => {
    return cartItems.reduce((total, item) => total + (item.product.price * item.quantity), 0);
  };

  const getCartItemsCount = () => {
    return cartItems.reduce((count, item) => count + item.quantity, 0);
  };

  return {
    cartItems,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    getCartTotal,
    getCartItemsCount,
  };
};
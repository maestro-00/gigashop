import { useState, useEffect } from "react";
import { CartItem, Product, Color } from "@/types/product";
import { useApi } from "./use-api";

export const useCart = () => {
  const { request, loading } = useApi("basket-service");
  const [cartItems, setCartItems] = useState<CartItem[]>([]);
  const [userName, setUserName] = useState<string | null>(localStorage.getItem('username'));

  useEffect(() => {
    if (userName) {
      (async () => {
        const data = await request<CartItem[]>({
          url: `/basket/${userName}`,
          method: "GET",
        });
        if (data) setCartItems(data.basket.items);
      })();
    }
  }, []);

  const addToCart = async (
    product: Product,
    selectedColor: Color,
    selectedSize: string,
    quantity: number = 1
  ) => {
    let currentUsername = userName;
    if (!userName) {
      currentUsername = crypto.randomUUID();
      setUserName(currentUsername);
      localStorage.setItem("username", currentUsername);
    }
    let newCartItems: CartItem[] = [];
    const existingItem = cartItems.find(
      (item) =>
        item.product.id === product.id &&
        item.color.value === selectedColor.value &&
        item.size === selectedSize
    );
    if (existingItem) {
      newCartItems = cartItems.map((item) =>
        item === existingItem
          ? { ...item, quantity: item.quantity + quantity }
          : item
      );
    } else {
      newCartItems.push({
        product, 
        color: selectedColor, 
        size: selectedSize,
        quantity: quantity,
      });
    }
    await postCart(newCartItems,currentUsername);
  };

  const removeFromCart = async (cartItemId: string) => {
    const newCartItems = cartItems.filter((i) => i.product.id !== cartItemId);
    await postCart(newCartItems);
  };

  const updateQuantity = async (cartItemId: string, quantity: number) => {
    const newCartItems = cartItems.map((i) => {
      if (i.product.id === cartItemId) {
        i.quantity = quantity;
      }
      return i;
    });
    await postCart(newCartItems);
  };

  const clearCart = async () => {
    await request({ url: `/basket/${userName}`, method: "DELETE" });
    setCartItems([]);
  };

  const postCart = async (cartItems: CartItem[], currentUsername: string = userName) => { 
    const res = await request<CartItem>({
      url: "/basket",
      method: "POST",
     data: {Cart: { userName: currentUsername, items: cartItems }
  }});
    if (res) {
      setCartItems(cartItems);
    }
  };

  const getCartItemsCount = () => {
    return cartItems.reduce((count, item) => count + item.quantity, 0);
  };

  const getCartTotal = () => {
    return cartItems.reduce(
      (total, item) => total + item.product.price * item.quantity,
      0
    );
  };

  return {
    cartItems,
    loading,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    getCartItemsCount,
    getCartTotal,
  };
};

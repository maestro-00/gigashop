import { useState, useEffect, createContext, useContext, ReactNode } from "react";
import { CartItem, Product, Color } from "@/types/product";
import { useApi } from "./use-api";

interface CartContextType {
  cartItems: CartItem[];
  loading: boolean;
  addToCart: (
    product: Product,
    selectedColor: Color,
    selectedSize: string,
    quantity?: number
  ) => Promise<void>;
  removeFromCart: (cartItemId: string) => Promise<void>;
  updateQuantity: (cartItemId: string, quantity: number) => Promise<void>;
  clearCart: () => Promise<void>;
  getCartItemsCount: () => number;
  getCartTotal: () => number;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const CartProvider = ({ children }: { children: ReactNode }) => {
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
  }, [userName]);

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
      newCartItems = [...cartItems];
      newCartItems.push({
        product, 
        color: selectedColor, 
        size: selectedSize,
        quantity: quantity,
      });
    }
    await postCart(newCartItems, currentUsername);
  };

  const removeFromCart = async (cartItemId: string) => {
    const newCartItems = cartItems.filter(i => i.product.id !== cartItemId);
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
      data: {Cart: { userName: currentUsername, items: cartItems } }
    });
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

  return (
    <CartContext.Provider value={{
      cartItems,
      loading,
      addToCart,
      removeFromCart,
      updateQuantity,
      clearCart,
      getCartItemsCount,
      getCartTotal,
    }}>
      {children}
    </CartContext.Provider>
  );
};

export const useCart = () => {
  const context = useContext(CartContext);
  if (context === undefined) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
};

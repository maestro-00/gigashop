import { useState } from "react";
import axios, { AxiosRequestConfig } from "axios";

type ServiceName = "basket-service" | "product-service" | "user-service";

const serviceUrls: Record<ServiceName, string> = {
  "basket-service": import.meta.env.VITE_CART_SERVICE_URL || "http://localhost:5001",
  "product-service": import.meta.env.VITE_PRODUCT_SERVICE_URL || "http://localhost:5002",
  "user-service": import.meta.env.VITE_USER_SERVICE_URL || "http://localhost:5003",
};

export const useApi = (service: ServiceName) => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<unknown>(null);

  const baseURL = serviceUrls[service];

  const request = async <T = any>(config: AxiosRequestConfig) => {
    try {
      setLoading(true);
      setError(null);
      const res = await axios({ baseURL, ...config });
      return res.data;
    } catch (err) {
      setError(err);
      console.error("API Error:", err);
      return null;
    } finally {
      setLoading(false);
    }
  };

  return {
    request,
    loading,
    error,
  };
};

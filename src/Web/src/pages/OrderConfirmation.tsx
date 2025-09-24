import React, { useEffect, useState } from 'react';
import { useSearchParams, Link, useNavigate } from 'react-router-dom';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { CheckCircle, Package, Truck, CreditCard } from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

export default function OrderConfirmation() {
  const [searchParams] = useSearchParams();
  const [order, setOrder] = useState<any>(null);
  const [loading, setLoading] = useState(true); 
  const { toast } = useToast();
  const navigate = useNavigate();

  const sessionId = searchParams.get('session_id');

  useEffect(() => {
    const completeOrder = async () => {
      if (!sessionId) {
        toast({
          title: 'Invalid Order',
          description: 'No session found. Redirecting to home...',
          variant: 'destructive'
        });
        setTimeout(() => navigate('/'), 3000);
        return;
      }

      try {
        // Complete the order processing
        // const { data, error } = await supabase.functions.invoke('complete-order', {
        //   body: { sessionId }
        // });

        // if (error) throw error;

        // setOrder(data.order);
        
        // Clear cart from localStorage
        localStorage.removeItem('cart');
        
        toast({
          title: 'Order Confirmed!',
          description: 'Your payment was successful and your order has been placed.'
        });
      } catch (error) {
        console.error('Error completing order:', error);
        toast({
          title: 'Order Processing Error',
          description: 'There was an issue processing your order. Please contact support.',
          variant: 'destructive'
        });
      } finally {
        setLoading(false);
      }
    };

    completeOrder();
  }, [sessionId, toast, navigate]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary mx-auto mb-4"></div>
          <p className="text-lg">Processing your order...</p>
        </div>
      </div>
    );
  }

  if (!order) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-2xl font-bold mb-4">Order Not Found</h1>
          <p className="text-muted-foreground mb-4">We couldn't find your order details.</p>
          <Button asChild>
            <Link to="/">Return to Home</Link>
          </Button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background py-8">
      <div className="container mx-auto px-4 max-w-4xl">
        <div className="text-center mb-8">
          <CheckCircle className="h-16 w-16 text-green-500 mx-auto mb-4" />
          <h1 className="text-3xl font-bold mb-2">Order Confirmed!</h1>
          <p className="text-lg text-muted-foreground">
            Thank you for your purchase. Your order has been successfully placed.
          </p>
        </div>

        <div className="grid gap-6 md:grid-cols-2">
          {/* Order Details */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Package className="h-5 w-5" />
                Order Details
              </CardTitle>
              <CardDescription>Order #{order.id.slice(0, 8)}</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex justify-between">
                <span>Status:</span>
                <Badge variant="secondary">{order.status}</Badge>
              </div>
              <div className="flex justify-between">
                <span>Total:</span>
                <span className="font-semibold">${order.total_amount.toFixed(2)}</span>
              </div>
              <div className="flex justify-between">
                <span>Payment Status:</span>
                <Badge variant="default">
                  <CreditCard className="h-3 w-3 mr-1" />
                  {order.payment_status}
                </Badge>
              </div>
            </CardContent>
          </Card>

          {/* Shipping Information */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Truck className="h-5 w-5" />
                Shipping Information
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-2 text-sm">
                <p className="font-semibold">
                  {order.shipping_first_name} {order.shipping_last_name}
                </p>
                <p>{order.shipping_street}</p>
                <p>
                  {order.shipping_city}, {order.shipping_state} {order.shipping_zip_code}
                </p>
                <p>{order.shipping_country}</p>
                <p className="text-muted-foreground mt-2">{order.shipping_email}</p>
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Action Buttons */}
        <div className="mt-8 flex flex-col sm:flex-row gap-4 justify-center">
          <Button asChild>
            <Link to="/">Continue Shopping</Link>
          </Button>
            <Button variant="outline" asChild>
              <Link to="/order-history">View Order History</Link>
            </Button>
        </div>

        {/* What's Next */}
        <Card className="mt-8">
          <CardHeader>
            <CardTitle>What happens next?</CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <div className="flex items-start gap-3">
              <div className="h-2 w-2 bg-primary rounded-full mt-2"></div>
              <div>
                <p className="font-semibold">Order Processing</p>
                <p className="text-sm text-muted-foreground">
                  We're preparing your items for shipment.
                </p>
              </div>
            </div>
            <div className="flex items-start gap-3">
              <div className="h-2 w-2 bg-muted rounded-full mt-2"></div>
              <div>
                <p className="font-semibold">Shipping Confirmation</p>
                <p className="text-sm text-muted-foreground">
                  You'll receive a tracking number via email once your order ships.
                </p>
              </div>
            </div>
            <div className="flex items-start gap-3">
              <div className="h-2 w-2 bg-muted rounded-full mt-2"></div>
              <div>
                <p className="font-semibold">Delivery</p>
                <p className="text-sm text-muted-foreground">
                  Your order will arrive within 3-5 business days.
                </p>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
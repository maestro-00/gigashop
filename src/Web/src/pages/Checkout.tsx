import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '@/hooks/useCart';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { Badge } from '@/components/ui/badge';
import { Checkbox } from '@/components/ui/checkbox';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { ArrowLeft, CreditCard, MapPin, Truck } from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

const Checkout = () => {
  const { cartItems, getCartTotal } = useCart();
  const { toast } = useToast();
  const navigate = useNavigate();
  const [sameAsShipping, setSameAsShipping] = useState(true);
  const [loading, setLoading] = useState(false);
  const [shippingForm, setShippingForm] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    street: '',
    city: '',
    state: '',
    zipCode: '',
    country: 'US'
  });
  const [billingForm, setBillingForm] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    street: '',
    city: '',
    state: '',
    zipCode: '',
    country: 'US'
  });

  const handleShippingChange = (field: string, value: string) => {
    setShippingForm(prev => ({ ...prev, [field]: value }));
  };

  const handleBillingChange = (field: string, value: string) => {
    setBillingForm(prev => ({ ...prev, [field]: value }));
  };

  const validateForm = () => {
    const requiredShippingFields = ['firstName', 'lastName', 'email', 'street', 'city', 'state', 'zipCode'];
    const shippingValid = requiredShippingFields.every(field => shippingForm[field as keyof typeof shippingForm].trim() !== '');
    
    if (!shippingValid) {
      toast({
        title: 'Missing Information',
        description: 'Please fill in all shipping address fields.',
        variant: 'destructive'
      });
      return false;
    }

    if (!sameAsShipping) {
      const requiredBillingFields = ['firstName', 'lastName', 'street', 'city', 'state', 'zipCode'];
      const billingValid = requiredBillingFields.every(field => billingForm[field as keyof typeof billingForm].trim() !== '');
      
      if (!billingValid) {
        toast({
          title: 'Missing Information',
          description: 'Please fill in all billing address fields.',
          variant: 'destructive'
        });
        return false;
      }
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;

    setLoading(true);

    try {
      const billingAddress = sameAsShipping ? shippingForm : billingForm;

      //Create Order

    //   const { data, error } = await supabase.functions.invoke('create-payment', {
    //     body: {
    //       cartItems,
    //       shippingAddress: shippingForm,
    //       billingAddress
    //     },
    //     headers: session ? {
    //       Authorization: `Bearer ${session.access_token}`
    //     } : {}
    //   });

    //   if (error) throw error;

      // Redirect to Stripe Checkout
    //   window.open(data.url, '_blank');
      
      toast({
        title: 'Redirecting to Payment',
        description: 'You will be redirected to complete your payment securely.'
      });

    } catch (error) {
      console.error('Payment error:', error);
      toast({
        title: 'Payment Error',
        description: 'There was an issue processing your payment. Please try again.',
        variant: 'destructive'
      });
    } finally {
      setLoading(false);
    }
  };

  if (cartItems.length === 0) {
    return (
      <div className="min-h-screen bg-background flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-2xl font-bold mb-4">Your cart is empty</h1>
          <Button onClick={() => navigate('/')}>Continue Shopping</Button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <div className="container mx-auto px-4 py-8">
        <div className="flex items-center gap-4 mb-8">
          <Button
            variant="ghost"
            size="icon"
            onClick={() => navigate('/')}
            className="h-10 w-10"
          >
            <ArrowLeft className="h-5 w-5" />
          </Button>
          <h1 className="text-3xl font-bold">Checkout</h1>
        </div>

        <div className="grid lg:grid-cols-2 gap-8">
          {/* Forms Section */}
          <div className="space-y-6">
            {/* Shipping Address */}
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <Truck className="h-5 w-5" />
                  Shipping Address
                </CardTitle>
              </CardHeader>
              <CardContent>
                <form className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label htmlFor="shipping-firstName">First Name</Label>
                      <Input id="shipping-firstName" required />
                    </div>
                    <div>
                      <Label htmlFor="shipping-lastName">Last Name</Label>
                      <Input id="shipping-lastName" required />
                    </div>
                  </div>
                  <div>
                    <Label htmlFor="shipping-email">Email</Label>
                    <Input id="shipping-email" type="email" required />
                  </div>
                  <div>
                    <Label htmlFor="shipping-phone">Phone</Label>
                    <Input id="shipping-phone" type="tel" required />
                  </div>
                  <div>
                    <Label htmlFor="shipping-street">Street Address</Label>
                    <Input id="shipping-street" required />
                  </div>
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label htmlFor="shipping-city">City</Label>
                      <Input id="shipping-city" required />
                    </div>
                    <div>
                      <Label htmlFor="shipping-state">State</Label>
                      <Select>
                        <SelectTrigger>
                          <SelectValue placeholder="Select state" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="CA">California</SelectItem>
                          <SelectItem value="NY">New York</SelectItem>
                          <SelectItem value="TX">Texas</SelectItem>
                          <SelectItem value="FL">Florida</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label htmlFor="shipping-zipCode">ZIP Code</Label>
                      <Input id="shipping-zipCode" required />
                    </div>
                    <div>
                      <Label htmlFor="shipping-country">Country</Label>
                      <Select defaultValue="US">
                        <SelectTrigger>
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="US">United States</SelectItem>
                          <SelectItem value="CA">Canada</SelectItem>
                          <SelectItem value="MX">Mexico</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>
                </form>
              </CardContent>
            </Card>

            {/* Billing Address */}
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <MapPin className="h-5 w-5" />
                  Billing Address
                </CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  <div className="flex items-center space-x-2">
                    <Checkbox
                      id="same-as-shipping"
                      checked={sameAsShipping}
                      onCheckedChange={(checked) => setSameAsShipping(checked as boolean)}
                    />
                    <Label htmlFor="same-as-shipping">Same as shipping address</Label>
                  </div>
                  
                  {!sameAsShipping && (
                    <form className="space-y-4">
                      <div className="grid grid-cols-2 gap-4">
                        <div>
                          <Label htmlFor="billing-firstName">First Name</Label>
                          <Input id="billing-firstName" required />
                        </div>
                        <div>
                          <Label htmlFor="billing-lastName">Last Name</Label>
                          <Input id="billing-lastName" required />
                        </div>
                      </div>
                      <div>
                        <Label htmlFor="billing-street">Street Address</Label>
                        <Input id="billing-street" required />
                      </div>
                      <div className="grid grid-cols-2 gap-4">
                        <div>
                          <Label htmlFor="billing-city">City</Label>
                          <Input id="billing-city" required />
                        </div>
                        <div>
                          <Label htmlFor="billing-state">State</Label>
                          <Select>
                            <SelectTrigger>
                              <SelectValue placeholder="Select state" />
                            </SelectTrigger>
                            <SelectContent>
                              <SelectItem value="CA">California</SelectItem>
                              <SelectItem value="NY">New York</SelectItem>
                              <SelectItem value="TX">Texas</SelectItem>
                              <SelectItem value="FL">Florida</SelectItem>
                            </SelectContent>
                          </Select>
                        </div>
                      </div>
                      <div className="grid grid-cols-2 gap-4">
                        <div>
                          <Label htmlFor="billing-zipCode">ZIP Code</Label>
                          <Input id="billing-zipCode" required />
                        </div>
                        <div>
                          <Label htmlFor="billing-country">Country</Label>
                          <Select defaultValue="US">
                            <SelectTrigger>
                              <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                              <SelectItem value="US">United States</SelectItem>
                              <SelectItem value="CA">Canada</SelectItem>
                              <SelectItem value="MX">Mexico</SelectItem>
                            </SelectContent>
                          </Select>
                        </div>
                      </div>
                    </form>
                  )}
                </div>
              </CardContent>
            </Card>

            {/* Payment Information */}
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <CreditCard className="h-5 w-5" />
                  Payment Information
                </CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-center p-6 bg-muted/50 rounded-lg">
                  <CreditCard className="h-8 w-8 mx-auto mb-2 text-muted-foreground" />
                  <p className="text-sm text-muted-foreground">
                    Payment will be processed securely through Stripe Checkout after clicking "Place Order"
                  </p>
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Order Summary */}
          <div>
            <Card className="sticky top-8">
              <CardHeader>
                <CardTitle>Order Summary</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  {cartItems.map((item) => (
                    <div key={`${item.product.id}-${item.selectedColor.value}-${item.selectedSize}`} className="flex gap-3">
                      <img
                        src={item.product.images[0]}
                        alt={item.product.name}
                        className="w-16 h-16 object-cover rounded"
                      />
                      <div className="flex-1 min-w-0">
                        <h4 className="font-medium text-sm truncate">{item.product.name}</h4>
                        <div className="flex items-center gap-2 mt-1">
                          <div
                            className="w-3 h-3 rounded border"
                            style={{ backgroundColor: item.selectedColor.value }}
                          />
                          <span className="text-xs text-muted-foreground">
                            {item.selectedColor.name}
                          </span>
                          {item.selectedSize && (
                            <Badge variant="secondary" className="text-xs">
                              {item.selectedSize}
                            </Badge>
                          )}
                        </div>
                        <div className="flex justify-between items-center mt-1">
                          <span className="text-sm">Qty: {item.quantity}</span>
                          <span className="font-medium text-sm">
                            ${(item.product.price * item.quantity).toFixed(2)}
                          </span>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
                
                <Separator className="my-4" />
                
                <div className="space-y-2">
                  <div className="flex justify-between text-sm">
                    <span>Subtotal</span>
                    <span>${getCartTotal().toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between text-sm">
                    <span>Shipping</span>
                    <span>Free</span>
                  </div>
                  <div className="flex justify-between text-sm">
                    <span>Tax</span>
                    <span>${(getCartTotal() * 0.08).toFixed(2)}</span>
                  </div>
                  <Separator />
                  <div className="flex justify-between font-bold">
                    <span>Total</span>
                    <span>${(getCartTotal() * 1.08).toFixed(2)}</span>
                  </div>
                </div>
                
                <Button
                  className="w-full mt-6 bg-shop-accent hover:bg-shop-accent-hover text-shop-accent-foreground"
                  onClick={handleSubmit}
                  disabled={loading}
                >
                  {loading ? 'Processing...' : 'Place Order'}
                </Button>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Checkout;
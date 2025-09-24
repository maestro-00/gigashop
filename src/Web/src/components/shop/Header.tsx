import { useState } from 'react';
import { Link } from 'react-router-dom';
import { Search, ShoppingCart, User, Menu, X, LogOut, Package, LogIn } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Badge } from '@/components/ui/badge';
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from '@/components/ui/dropdown-menu';
import { useCart } from '@/hooks/useCart';
import { CartDrawer } from './CartDrawer';

export const Header = () => {
  const [isCartOpen, setIsCartOpen] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const { getCartItemsCount } = useCart(); 
  const cartItemsCount = getCartItemsCount();

  return (
    <>
      <header className="bg-background border-b sticky top-0 z-50">
        <div className="container mx-auto px-4">
          <div className="flex items-center justify-between h-16">
            {/* Logo */}
            <div className="flex items-center">
              <h1 className="text-2xl font-bold bg-gradient-shop bg-clip-text text-transparent">
                GigaShop
              </h1>
            </div>

            {/* Desktop Navigation */}
            <nav className="hidden md:flex items-center space-x-8">
              <Link to="/" className="text-foreground hover:text-shop-accent transition-colors">
                Home
              </Link>
              <Link to="/products" className="text-foreground hover:text-shop-accent transition-colors">
                Products
              </Link>
              <Link to="/categories" className="text-foreground hover:text-shop-accent transition-colors">
                Categories
              </Link>
              <Link to="/about" className="text-foreground hover:text-shop-accent transition-colors">
                About
              </Link>
            </nav>

            {/* Search Bar */}
            <div className="hidden md:flex items-center flex-1 max-w-md mx-8">
              <div className="relative w-full">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground h-4 w-4" />
                <Input
                  placeholder="Search products..."
                  className="pl-10"
                />
              </div>
            </div>

            {/* Actions */}
            <div className="flex items-center space-x-4">
             
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon" className="hidden md:flex">
                      <User className="h-5 w-5" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end" className="w-48">
                    <DropdownMenuItem asChild>
                      <Link to="/order-history" className="flex items-center">
                        <Package className="mr-2 h-4 w-4" />
                        Order History
                      </Link>
                    </DropdownMenuItem>
                    {/* <DropdownMenuSeparator />
                    <DropdownMenuItem className="flex items-center text-red-600">
                      <LogOut className="mr-2 h-4 w-4" />
                      Sign Out
                    </DropdownMenuItem> */}
                  </DropdownMenuContent>
                </DropdownMenu>  
              
              <Button
                variant="ghost"
                size="icon"
                className="relative"
                onClick={() => setIsCartOpen(true)}
              >
                <ShoppingCart className="h-5 w-5" />
                {cartItemsCount > 0 && (
                  <Badge
                    variant="secondary"
                    className="absolute -top-2 -right-2 h-5 w-5 flex items-center justify-center text-xs bg-shop-accent text-shop-accent-foreground"
                  >
                    {cartItemsCount}
                  </Badge>
                )}
              </Button>

              {/* Mobile Menu Button */}
              <Button
                variant="ghost"
                size="icon"
                className="md:hidden"
                onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
              >
                {isMobileMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
              </Button>
            </div>
          </div>

          {/* Mobile Menu */}
          {isMobileMenuOpen && (
            <div className="md:hidden py-4 border-t">
              <div className="flex flex-col space-y-4">
                <div className="relative">
                  <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground h-4 w-4" />
                  <Input
                    placeholder="Search products..."
                    className="pl-10"
                  />
                </div>
                
                <nav className="flex flex-col space-y-2">
                  <Link to="/" className="text-foreground hover:text-shop-accent transition-colors py-2">
                    Home
                  </Link>
                  <Link to="/products" className="text-foreground hover:text-shop-accent transition-colors py-2">
                    Products
                  </Link>
                  <Link to="/categories" className="text-foreground hover:text-shop-accent transition-colors py-2">
                    Categories
                  </Link>
                  <Link to="/about" className="text-foreground hover:text-shop-accent transition-colors py-2">
                    About
                  </Link>
                </nav>
                
                  <div className="space-y-2">
                    <Button variant="outline" className="justify-start w-full" asChild>
                      <Link to="/order-history">
                        <Package className="h-4 w-4 mr-2" />
                        Order History
                      </Link>
                    </Button>
                    {/* <Button variant="outline" className="justify-start w-full text-red-600" onClick={handleSignOut}>
                      <LogOut className="h-4 w-4 mr-2" />
                      Sign Out
                    </Button> */}
                  </div>
              
              </div>
            </div>
          )}
        </div>
      </header>

      <CartDrawer isOpen={isCartOpen} onClose={() => setIsCartOpen(false)} />
    </>
  );
};
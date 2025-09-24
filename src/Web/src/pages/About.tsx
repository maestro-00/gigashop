import { Header } from '@/components/shop/Header';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { Star, Users, Award, Globe, Heart, Shield, Truck, Headphones } from 'lucide-react';

const About = () => {
  const stats = [
    { icon: Users, label: 'Happy Customers', value: '50K+' },
    { icon: Globe, label: 'Countries Served', value: '25+' },
    { icon: Award, label: 'Years Experience', value: '10+' },
    { icon: Star, label: 'Average Rating', value: '4.9' }
  ];

  const values = [
    {
      icon: Heart,
      title: 'Customer First',
      description: 'Every decision we make starts with our customers in mind. Your satisfaction is our priority.'
    },
    {
      icon: Shield,
      title: 'Quality Assured',
      description: 'We carefully curate every product to ensure the highest standards of quality and reliability.'
    },
    {
      icon: Truck,
      title: 'Fast Delivery',
      description: 'Quick and reliable shipping to get your orders to you as soon as possible.'
    },
    {
      icon: Headphones,
      title: '24/7 Support',
      description: 'Our dedicated support team is always here to help you with any questions or concerns.'
    }
  ];

  const teamMembers = [
    {
      name: 'Sarah Johnson',
      role: 'Founder & CEO',
      image: '/placeholder.svg',
      description: 'Passionate about creating exceptional shopping experiences.'
    },
    {
      name: 'Michael Chen',
      role: 'Head of Product',
      image: '/placeholder.svg',
      description: 'Expert in product curation and quality assurance.'
    },
    {
      name: 'Emily Rodriguez',
      role: 'Customer Success',
      image: '/placeholder.svg',
      description: 'Dedicated to ensuring every customer has an amazing experience.'
    }
  ];

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      <main className="container mx-auto px-4 py-8">
        {/* Hero Section */}
        <div className="text-center mb-16">
          <h1 className="text-4xl font-bold mb-6 bg-gradient-shop bg-clip-text text-transparent">
            About GigaShop
          </h1>
          <p className="text-xl text-muted-foreground max-w-3xl mx-auto leading-relaxed">
            We're more than just an online store. We're a community of people passionate about bringing you 
            the finest products with exceptional service. Founded in 2014, GigaShop has grown from a small 
            startup to a trusted global marketplace.
          </p>
        </div>

        {/* Stats Section */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-6 mb-16">
          {stats.map((stat) => (
            <Card key={stat.label} className="text-center">
              <CardContent className="pt-6">
                <stat.icon className="h-8 w-8 mx-auto mb-4 text-shop-accent" />
                <div className="text-3xl font-bold text-shop-accent mb-2">{stat.value}</div>
                <div className="text-sm text-muted-foreground">{stat.label}</div>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Story Section */}
        <div className="mb-16">
          <Card>
            <CardHeader>
              <CardTitle className="text-2xl">Our Story</CardTitle>
            </CardHeader>
            <CardContent className="prose prose-neutral dark:prose-invert max-w-none">
              <p className="text-muted-foreground leading-relaxed mb-4">
                GigaShop began with a simple idea: everyone deserves access to high-quality products 
                at fair prices. Our founder, Sarah Johnson, started the company after struggling to 
                find reliable online retailers that truly cared about their customers.
              </p>
              <p className="text-muted-foreground leading-relaxed mb-4">
                What started as a small operation in a garage has now grown into a global marketplace 
                serving customers in over 25 countries. We've maintained our core values while scaling 
                our operations, always putting customer satisfaction first.
              </p>
              <p className="text-muted-foreground leading-relaxed">
                Today, we're proud to offer carefully curated products from trusted suppliers around 
                the world, backed by our commitment to quality, service, and value.
              </p>
            </CardContent>
          </Card>
        </div>

        {/* Values Section */}
        <div className="mb-16">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-bold mb-4">Our Values</h2>
            <p className="text-muted-foreground text-lg">
              The principles that guide everything we do
            </p>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            {values.map((value) => (
              <Card key={value.title} className="text-center hover:shadow-lg transition-shadow">
                <CardHeader>
                  <value.icon className="h-12 w-12 mx-auto mb-4 text-shop-accent" />
                  <CardTitle className="text-lg">{value.title}</CardTitle>
                </CardHeader>
                <CardContent>
                  <CardDescription>{value.description}</CardDescription>
                </CardContent>
              </Card>
            ))}
          </div>
        </div>

        {/* Team Section */}
        <div className="mb-16">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-bold mb-4">Meet Our Team</h2>
            <p className="text-muted-foreground text-lg">
              The passionate people behind GigaShop
            </p>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {teamMembers.map((member) => (
              <Card key={member.name} className="text-center">
                <CardContent className="pt-6">
                  <div className="w-24 h-24 rounded-full bg-muted mx-auto mb-4 overflow-hidden">
                    <img
                      src={member.image}
                      alt={member.name}
                      className="w-full h-full object-cover"
                    />
                  </div>
                  <h3 className="font-semibold text-lg mb-1">{member.name}</h3>
                  <Badge variant="secondary" className="mb-3">{member.role}</Badge>
                  <p className="text-muted-foreground text-sm">{member.description}</p>
                </CardContent>
              </Card>
            ))}
          </div>
        </div>

        {/* Mission Section */}
        <Card className="mb-16">
          <CardHeader>
            <CardTitle className="text-2xl text-center">Our Mission</CardTitle>
          </CardHeader>
          <CardContent>
            <blockquote className="text-center text-lg italic text-muted-foreground border-l-4 border-shop-accent pl-6 py-4">
              "To make premium quality products accessible to everyone, while building lasting 
              relationships with our customers through exceptional service and genuine care."
            </blockquote>
            <div className="text-center mt-6">
              <cite className="text-sm text-muted-foreground">— Sarah Johnson, Founder & CEO</cite>
            </div>
          </CardContent>
        </Card>

        {/* Contact CTA */}
        <div className="text-center">
          <h2 className="text-2xl font-bold mb-4">Get in Touch</h2>
          <p className="text-muted-foreground mb-6">
            Have questions? We'd love to hear from you. Send us a message and we'll respond as soon as possible.
          </p>
          <Button size="lg" className="bg-shop-accent hover:bg-shop-accent-hover text-shop-accent-foreground">
            Contact Us
          </Button>
        </div>
      </main>
    </div>
  );
};

export default About;
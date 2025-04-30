import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem, CartType } from '../../shared/models/Cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  baseUrl = environment.apiUrl; // Base URL for the API

  http = inject(HttpClient); // Inject HttpClient for making HTTP requests

  cart = signal<Cart | null>(null); // Signal to hold the current cart state
  
  itemCount = computed(() =>{
    return (this.cart()?.items ?? []).reduce((sum, item) => sum + item.quantity, 0);
  });
  totals = computed(() => {
    const cart = this.cart();
    if (!cart) return null; // Return 0 if cart is null
    const subtotal = cart.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const shipping = subtotal > 10000 ? 0 : 5000; // Shipping cost based on subtotal
    const discount = 0;
    return {
      subtotal,
      shipping,
      discount,
      total: subtotal + shipping - discount
    }

  });
  
  getCart(id : string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(
        cart => {
          this.cart.set(cart)
          return cart; // Update the cart signal with the fetched cart data
        }
      )); // Fetch cart by ID
  }
  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe(
      {
        next: cart => this.cart.set(cart), // Update the cart signal with the new cart data
      }
    ) // Send the cart data to the server
  }

  addItemToCart(item: CartItem|Product , quantity = 1) {
    const cart = this.cart() ?? this.createCart(); 
     if(this.isProduct(item)) {
      item = this.mapProductToCartItem(item); // Map product to cart item structure
      }
      cart.items = this.addOrUpdateItem(cart.items ?? [], item, quantity);
      this.setCart(cart); // Send the updated cart to the server
}

private addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
  const index = items.findIndex(x => x.productId === item.productId);
  if (index === -1) {
    item.quantity = quantity;
    items.push(item);
  } else {
    items[index].quantity += quantity
  }
  return items;
}

  private isProduct(item: CartItem|Product): item is Product {
    return (item as Product).id !== undefined; // Check if the item is a product
  }

  private mapProductToCartItem(product: Product): CartItem {
    return {
      productId: product.id,
      productName: product.name,
      price: product.price,
      quantity: 0,
      pictureUrl: product.pictureUrl,
      brand: product.brand,
      type: product.type
    }; // Map product details to cart item structure
  }


  private createCart(): Cart  {
    const cart = new Cart(); // Create a new cart instance
    localStorage.setItem('cart_id', cart.id); // Store the cart ID in local storage
    return cart; // Return the new cart instance
  }

  removeItemFromCart(id:number , quantity = 1) {
    const cart = this.cart() 
    if (!cart) return ; // Return if cart is null
    const index = cart.items.findIndex(x => x.productId === id); // Find the index of the item to remove
    if (index != -1) {
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity -= quantity; // Decrease the quantity if more than 1
      } else {
        cart.items.splice(index, 1); // Remove the item from the cart
      }
      if(cart.items.length === 0) {
        this.deleteCart(); // Remove cart ID from local storage if cart is empty
      }
      else{
        this.setCart(cart); // Send the updated cart to the server
      }
  }}
  deleteCart() {
    this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe(
      () => {
        localStorage.removeItem('cart_id'); // Remove cart ID from local storage
        this.cart.set(null); // Clear the cart signal
      })
     
  }

}

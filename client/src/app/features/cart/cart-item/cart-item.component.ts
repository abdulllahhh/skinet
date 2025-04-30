import { Component, inject, input } from '@angular/core';
import { CartItem } from '../../../shared/models/Cart';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [
    RouterLink,
    MatButton,
    MatIcon,
    CurrencyPipe
  ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.scss'
})
export class CartItemComponent {
  item = input.required<CartItem>();
  cartService = inject(CartService); 

  incrementQuantity() {
    this.cartService.addItemToCart(this.item()); // Increment item quantity in cart
  }
  decrementQuantity() {
    this.cartService.removeItemFromCart(this.item().productId); // Decrement item quantity in cart
  }
  removeItemFromCart() {
    this.cartService.removeItemFromCart(this.item().productId, this.item().quantity); // Remove item from cart
  }
}

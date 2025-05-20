import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

export const emptyCartGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const router = inject(Router);
  const snackbar = inject(MatSnackBar);

  if (cartService.itemCount()) {
    return true;
  } else {
    snackbar.open('Your cart is empty. Please add items to your cart before proceeding to checkout.', 'Close', {
      duration: 5000,
      panelClass: ['snackbar-error']
    });
    //router.navigate(['/cart']);
    return false;
  }
};
 
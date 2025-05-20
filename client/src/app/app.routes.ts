import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetailsComponent } from './features/shop/product-details/product-details.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { CartComponent } from './features/cart/cart.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { authGuard } from './core/guards/auth.guard';
import { emptyCartGuard } from './core/guards/empty-cart.guard';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'shop', component: ShopComponent},
    {path: 'shop/:id', component: ProductDetailsComponent}, // Dynamic route for product details
    {path: 'test-error', component: TestErrorComponent},  
    {path: 'checkout', component: CheckoutComponent, canActivate: [authGuard, emptyCartGuard]}, // Checkout route with AuthGuard
    {path: 'account/login', component: LoginComponent}, // Login route
    {path: 'account/register', component: RegisterComponent}, // Register route
    {path: 'cart', component: CartComponent },
    {path: 'not-found', component: NotFoundComponent},  
    {path: 'server-error', component: ServerErrorComponent},  
    {path: '**', redirectTo: 'not-found', pathMatch:'full'} // Wildcard route for a 404 page
];

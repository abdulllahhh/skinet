import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatDivider } from '@angular/material/divider';
import { CurrencyPipe } from '@angular/common';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    MatFormField,
    MatIcon,
    MatLabel,
    MatDivider,
    CurrencyPipe,
    MatInput
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  
  private shopService = inject(ShopService);
  private activatedRoute = inject(ActivatedRoute);

  product?: Product;

  ngOnInit(): void {
    this.loadProduct();
  }
  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.shopService.getProduct(+id).subscribe({
        next: (product) => this.product = product,
        error: (error) => console.error(error)
      });
    } else {
      console.error('Product ID not found in route parameters.');
    }
  }
}

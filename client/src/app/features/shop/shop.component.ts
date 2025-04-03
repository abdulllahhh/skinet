import { Component, inject, OnInit } from '@angular/core';
import { Product } from '../../shared/models/product';
import { ShopService } from '../../core/services/shop.service';
import{ MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatList, MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCardModule,
    CommonModule,
    ProductItemComponent,
    MatDialogModule,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  
  private shopService = inject(ShopService);  
  private dialogService = inject(MatDialog);
  products?: Pagination<Product> ;
  sortOptions = [
    { name: 'Alphabetical', value: 'Name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];
  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 20, 50];
  ngOnInit(): void {
    this.initializeShop();  
  }
initializeShop(){
  this.shopService.getTypes();
  this.shopService.getBrands();
  this.getProducts();
}
getProducts(){
  this.shopService.getProducts(this.shopParams).subscribe({
    next :  response => this.products = response,
    error: (error) => console.log(error),
  });
}
onSearchChange() {
  
  this.shopParams.pageNumber = 1;
  this.getProducts();
}  

handlePageEvent(event: PageEvent) {
  this.shopParams.pageNumber = event.pageIndex + 1;
  this.shopParams.pageSize = event.pageSize;
  this.getProducts();
}
onSortChange(event : MatSelectionListChange) {  
  const selectedOption = event.options[0].value;
  if(selectedOption) {
    this.shopParams.sort = selectedOption;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
}
openFiltersDialog() {
  const dialogRef = this.dialogService.open(FiltersDialogComponent, {
    minWidth: '500px',
    data: { 
      selectedTypes: this.shopParams.types,
      selectedBrands: this.shopParams.brands,
    }
  });
  dialogRef.afterClosed().subscribe((result) => {
    if (result) {
      this.shopParams.types = result.selectedTypes;
      this.shopParams.brands = result.selectedBrands;
      this.getProducts();
    }
  });
}
}

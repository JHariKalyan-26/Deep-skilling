import { CurrencyPipe } from '@angular/common';
import { Component, input, output } from '@angular/core';
import { Product } from '../../models/product';
import { StockStatusPipe } from '../../pipes/stock-status.pipe';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CurrencyPipe, StockStatusPipe],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css'
})
export class ProductCard {
  readonly product = input.required<Product>();
  readonly remove = output<number>();
}

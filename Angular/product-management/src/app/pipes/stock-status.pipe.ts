import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'stockStatus',
  standalone: true
})
export class StockStatusPipe implements PipeTransform {
  transform(stock: number): string {
    if (stock === 0) return 'Out of stock';
    if (stock < 10) return 'Low stock';
    return 'Available';
  }
}

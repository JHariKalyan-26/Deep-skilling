import { Injectable, signal } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { Product } from '../models/product';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly productsSignal = signal<Product[]>([
    { id: 1, name: 'Laptop', category: 'Electronics', price: 65000, stock: 10 },
    { id: 2, name: 'Keyboard', category: 'Accessories', price: 1500, stock: 25 },
    { id: 3, name: 'Mouse', category: 'Accessories', price: 800, stock: 40 }
  ]);

  readonly products = this.productsSignal.asReadonly();

  getAll(): Observable<Product[]> {
    return of(this.productsSignal()).pipe(
      delay(200),
      map(products => [...products])
    );
  }

  getById(id: number): Product | undefined {
    return this.productsSignal().find(product => product.id === id);
  }

  add(product: Omit<Product, 'id'>): void {
    const nextId = Math.max(0, ...this.productsSignal().map(item => item.id)) + 1;
    this.productsSignal.update(items => [...items, { id: nextId, ...product }]);
  }

  update(updated: Product): void {
    this.productsSignal.update(items =>
      items.map(item => item.id === updated.id ? updated : item)
    );
  }

  delete(id: number): void {
    this.productsSignal.update(items => items.filter(item => item.id !== id));
  }
}

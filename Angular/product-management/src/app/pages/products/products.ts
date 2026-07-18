import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductCard } from '../../components/product-card/product-card';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [FormsModule, ProductCard],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products {
  private readonly productService = inject(ProductService);

  readonly search = signal('');
  readonly products = this.productService.products;

  readonly filteredProducts = computed(() => {
    const term = this.search().trim().toLowerCase();

    if (!term) return this.products();

    return this.products().filter(product =>
      product.name.toLowerCase().includes(term) ||
      product.category.toLowerCase().includes(term)
    );
  });

  deleteProduct(id: number): void {
    this.productService.delete(id);
  }
}

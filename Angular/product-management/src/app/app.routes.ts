import { Routes } from '@angular/router';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/home/home').then(component => component.Home)
  },
  {
    path: 'products',
    loadComponent: () =>
      import('./pages/products/products').then(component => component.Products)
  },
  {
    path: 'add-product',
    canActivate: [adminGuard],
    loadComponent: () =>
      import('./pages/add-product/add-product').then(component => component.AddProduct)
  },
  {
    path: 'about',
    loadComponent: () =>
      import('./pages/about/about').then(component => component.About)
  },
  {
    path: '**',
    redirectTo: ''
  }
];

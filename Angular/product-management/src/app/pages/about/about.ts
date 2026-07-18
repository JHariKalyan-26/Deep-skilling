import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  standalone: true,
  template: `
    <section>
      <h1>About this module</h1>
      <p>
        This project demonstrates Angular 20 fundamentals using standalone
        components, routing, forms, dependency injection, signals and RxJS.
      </p>
    </section>
  `
})
export class About {}

import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Client');

  blogs:any[] = ["Blog1", "Blog2", "Blog3"];

  getBlogs() {
    return this.blogs;
  }
}

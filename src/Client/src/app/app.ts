import { Component, OnInit, signal } from '@angular/core';

declare var alertify: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  ngOnInit(): void {
    alertify.set('notifier','position', 'top-right');
   }
  protected readonly title = signal('Client');


}

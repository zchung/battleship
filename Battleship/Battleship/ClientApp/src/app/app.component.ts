import { Component } from '@angular/core';
import { SignalRService } from './Services/signalR-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  constructor(private signalRService: SignalRService) {
    this.signalRService.startConnection();
  }
}

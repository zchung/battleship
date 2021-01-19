import { Injectable, Inject } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Game } from '../models/game';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
private hubConnection: signalR.HubConnection;
private baseUrl: string;
 constructor(@Inject('BASE_URL') baseUrl: string) {
   this.baseUrl = baseUrl;
 }
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(`${this.baseUrl}game`)
                            .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  public addNewGameListener = (callBack: (game: Game) => void) => {
    this.hubConnection.on('sendnewgame', (data) => {
      if (callBack) {
        callBack(data);
      }
    });
  }
}

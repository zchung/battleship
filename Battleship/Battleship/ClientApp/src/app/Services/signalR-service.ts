import { Injectable, Inject } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { GameModel } from '../models/game-model';
import { UpdatedPlayer } from '../models/updated-player';
import { UpdatedGame } from '../models/updated-game-model';
import { Result } from '../models/result-data';
import { CoordinatesModel } from '../models/coordinates-model';
import { AttackingPlayerResult } from '../models/attacking-player-result';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
private hubConnection: signalR.HubConnection;
private baseUrl: string;
 constructor(@Inject('BASE_URL') baseUrl: string) {
   if (environment.production) {
    this.baseUrl = 'https://battleshipgame2021.herokuapp.com/';
   } else {
    this.baseUrl = baseUrl;
   }
 }
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(`${this.baseUrl}game`, {
                              skipNegotiation: true,
                              transport: signalR.HttpTransportType.WebSockets
                            })
                            .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  public addNewGameListener = (callBack: (game: GameModel) => void) => {
    this.hubConnection.on('sendnewgame', (data) => {
      if (callBack) {
        callBack(data);
      }
    });
  }

  public addRemoveGameListener = (callBack: (game: GameModel) => void) => {
    this.hubConnection.on('removegame', (data) => {
      if (callBack) {
        callBack(data);
      }
    });
  }

  public addPlayerJoinedGameListener = (callBack: (joinedPlayer: UpdatedPlayer) => void) => {
    this.hubConnection.on('sendplayerhasjoined', (data) => {
      if (callBack) {
        callBack(data);
      }
    });
  }

  public addPlayerIsReadyGameListener = (callBack: (updatedPlayer: UpdatedPlayer) => void) => {
    this.hubConnection.on('sendplayerisready', (data) => {
      if (callBack) {
        callBack(data);
      }
    });
  }

  public addUpdatedGameStatusListener = (callBack: (updatedGame: UpdatedGame) => void) => {
    this.hubConnection.on('sendupdategamestatus', (data) => {
      if (callBack) {
        callBack(data);
      }
    });
  }

  public addSendAttackPlayerCoordinatesListener = (callBack: (result: AttackingPlayerResult) => void) => {
    this.hubConnection.on('sendattackplayercoordinates', (data) => {
      if (callBack) {
        callBack(data);
      }
  });
  }
}

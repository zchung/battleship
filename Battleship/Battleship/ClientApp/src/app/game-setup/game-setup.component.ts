import { Component, OnInit } from "@angular/core";
import { GameStoreService } from '../Services/game-store-service';
import { GameModel } from '../models/game-model';
import { SignalRService } from '../Services/signalR-service';

@Component({
  selector: 'app-get-setup',
  templateUrl: './game-setup.component.html',
})
export class GameSetupComponent implements OnInit {
  public currentGame: GameModel;
  constructor(private gameStoreService: GameStoreService, private signalRService: SignalRService) {
    this.currentGame = this.gameStoreService.currentGame;
  }
  ngOnInit(): void {
    this.signalRService.addPlayerJoinedGameListener((joinedPlayer) => {
      if (this.currentGame.gameId === joinedPlayer.gameId) {
        alert(`Player ${joinedPlayer.playerId} has joined the game`);
      }
    })
  }
}

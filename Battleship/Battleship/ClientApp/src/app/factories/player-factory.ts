import { Injectable } from "@angular/core";
import { GameModel } from '../models/game-model';
import { IPlayer } from '../models/interfaces/player-interface';
import { Player1 } from '../models/player1-model';
import { Player2 } from '../models/player2-model';

@Injectable({
  providedIn: 'root'
})
export class PlayerFactory {
  getOtherPlayerFromGame(playerId: number, game: GameModel): IPlayer {
    if (playerId === 1) {
      return new Player2(2);
    } else if (playerId === 2) {
      return new Player1(1);
    }
  }

  getThisPlayerFromGame(playerId: number, game: GameModel): IPlayer {
    if (playerId === 1) {
      return new Player1(1);
    } else if (playerId == 2) {
      return new Player2(2);
    }
  }
}

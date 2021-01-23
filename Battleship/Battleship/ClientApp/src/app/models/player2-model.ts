import { PlayerStatus } from './enums/player-status';
import { GameModel } from './game-model';
import { IPlayer } from './interfaces/player-interface';

export class Player2 implements IPlayer {
  setPlayerStatus(game: GameModel, playerStatus: PlayerStatus) {
    game.player2Status = playerStatus;
  }
}

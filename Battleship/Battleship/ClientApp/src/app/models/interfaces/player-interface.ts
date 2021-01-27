import { GameModel } from '../game-model';
import { PlayerStatus } from '../enums/player-status';
export interface IPlayer {
  playerId: number;
  setPlayerStatus(game: GameModel, playerStatus: PlayerStatus);
}

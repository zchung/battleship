import { ShipModel } from './ship-model';
import { PlayerStatus } from './enums/player-status';
export class GameModel {
  gameId: number;
  description: string;
  playerId: number;
  ships: Array<ShipModel>;
  player1Status: PlayerStatus;
  player2Status: PlayerStatus;
  gameReadyToStart: boolean;
}

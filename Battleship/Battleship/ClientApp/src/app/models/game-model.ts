import { ShipModel } from './ship-model';
import { PlayerStatus } from './enums/player-status';
import { CoordinatesModel } from './coordinates-model';
export class GameModel {
  gameId: number;
  description: string;
  playerId: number;
  currentPlayerIdTurn: number;
  ships: Array<ShipModel>;
  attemptedCoordinates: Array<CoordinatesModel>;
  player1Status: PlayerStatus;
  player2Status: PlayerStatus;
  gameReadyToStart: boolean;
}

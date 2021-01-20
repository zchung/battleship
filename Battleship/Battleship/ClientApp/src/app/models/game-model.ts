import { ShipModel } from './ship-model';

export class GameModel {
  gameId: number;
  description: string;
  playerId: number;
  ships: Array<ShipModel>;
}

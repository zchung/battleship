import { CoordinatesModel } from './coordinates-model';
import { ShipType } from './enums/ship-type';

export class ShipModel {
  name: string;
  shipType: ShipType;
  size: number;
  coordinates: Array<CoordinatesModel>;
}

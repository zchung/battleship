import { CoordinatesModel } from '../coordinates-model';
import { ShipOrientationType } from '../enums/ship-orientation-type';
import { ShipType } from '../enums/ship-type';

export class UpdateShipPositionRequest {
  constructor (
    public gameId: number,
    public playerId: number,
    public shipType: ShipType,
    public startPosition: CoordinatesModel,
    public shipOrientationType: ShipOrientationType
  ) {}
}

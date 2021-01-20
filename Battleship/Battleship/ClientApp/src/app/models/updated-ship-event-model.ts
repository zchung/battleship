import { ShipModel } from './ship-model';
export class UpdatedShipEventModel {
  constructor(
  public previousShipModel: ShipModel,
  public newShipModel: ShipModel
  ) {}
}

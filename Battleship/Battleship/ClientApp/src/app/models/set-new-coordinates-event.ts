import { CoordinatesModel } from './coordinates-model';
import { CellType } from './enums/cell-type';

export class SetNewCoordinatesEvent {
  constructor(
  public coordinates: CoordinatesModel,
  public cellType: CellType) {}
}

import { CoordinatesModel } from './coordinates-model';
import { CellType } from './enums/cell-type';
export class CellModel {
  constructor(
  public text: string,
  public coordinates: CoordinatesModel,
  public cellType: CellType
  ) {}
}

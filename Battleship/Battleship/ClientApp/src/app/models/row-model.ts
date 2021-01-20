import { CellModel } from './cell-model';

export class RowModel {
  constructor(public cells: Array<CellModel>, public rowNumber) {}
}

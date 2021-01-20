import { Pipe, PipeTransform } from "@angular/core";
import { CellType } from "../models/enums/cell-type";
@Pipe({name: 'cellStyle'})
export class CellStylePipe implements PipeTransform {
  transform(cellType: CellType) {
    switch (cellType) {
      case CellType.Empty:
        return 'empty';
      case CellType.HasShip:
        return 'ship';
      default:
        return '';
    }
  }

}

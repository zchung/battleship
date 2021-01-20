import { EventEmitter, Injectable } from '@angular/core';
import { CellModel } from '../models/cell-model';
import { CoordinatesModel } from '../models/coordinates-model';
import { RowModel } from '../models/row-model';
import { CellType } from '../models/enums/cell-type';
import { ShipModel } from '../models/ship-model';
import { UpdatedShipEventModel } from '../models/updated-ship-event-model';

@Injectable({
  providedIn: 'root'
})
export class CoordinatesHelperService {
  public updatedShipEvent: EventEmitter<UpdatedShipEventModel> = new EventEmitter<UpdatedShipEventModel>();
  public findCellInRow(row: RowModel, xPosition: number): CellModel {
    return row.cells.find(x => x.coordinates !== null && x.coordinates.xPosition === xPosition);
  }

  public findRowInRows(rows: Array<RowModel>, yPosition: number): RowModel {
    return rows.find(x => x.rowNumber === yPosition);
  }

  public setCellTypeInRows(rows: Array<RowModel>, coordinates: Array<CoordinatesModel>, cellType: CellType): void {
    for (let index = 0; index < coordinates.length; index++) {
      const coordinate = coordinates[index];
      const row = this.findRowInRows(rows.filter(x => x.rowNumber > 0), coordinate.yPosition);
      const cell = this.findCellInRow(row, coordinate.xPosition);
      cell.cellType = cellType;
    }
  }

  public setCellTypeByShip(rows: Array<RowModel>, shipModel: ShipModel, cellType: CellType): void {
    this.setCellTypeInRows(rows, shipModel.coordinates, cellType);
  }
}

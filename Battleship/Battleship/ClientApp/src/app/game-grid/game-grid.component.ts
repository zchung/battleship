import { Component, OnInit, EventEmitter, Output, Input, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CellModel } from '../models/cell-model';
import { CoordinatesModel } from '../models/coordinates-model';
import { CellType } from '../models/enums/cell-type';
import { RowModel } from '../models/row-model';
import { ShipModel } from '../models/ship-model';
import { UpdatedShipEventModel } from '../models/updated-ship-event-model';
import { CoordinatesHelperService } from '../Services/coordinates-helper-service';
import { SetNewCoordinatesEvent } from '../models/set-new-coordinates-event';

@Component({
  selector: 'app-game-grid',
  templateUrl: './game-grid.component.html',
  styleUrls: ['./game-grid.component.scss']
})
export class GameGridComponent implements OnInit, OnDestroy {
  constructor(private coordinateHelperService: CoordinatesHelperService) {}


  public rows: Array<RowModel>;
  @Input() shipCoordinates: Array<CoordinatesModel>;
  @Input() setCellInGrid: EventEmitter<SetNewCoordinatesEvent>;
  @Input() isPlayerGrid: boolean = true;

  @Output() public cellSelectedEvent: EventEmitter<CoordinatesModel> = new EventEmitter<CoordinatesModel>();

  private unsubscribe: Subject<null> = new Subject();
  ngOnInit(): void {
    this.rows = this.buildGrid(this.shipCoordinates);
    this.coordinateHelperService.updatedShipEvent
    .pipe(
      takeUntil(this.unsubscribe)
    )
    .subscribe((eventModel: UpdatedShipEventModel) => {
      // remove old Ship
      this.coordinateHelperService.setCellTypeByShip(this.rows, eventModel.previousShipModel, CellType.Empty);
      // update new ship
      this.coordinateHelperService.setCellTypeByShip(this.rows, eventModel.newShipModel, CellType.HasShip);
    });

    if (this.setCellInGrid) {
      this.setCellInGrid
    .pipe(
      takeUntil(this.unsubscribe)
    )
    .subscribe((eventModel: SetNewCoordinatesEvent) => {
      this.coordinateHelperService.setCellTypeInRows(this.rows, [eventModel.coordinates], eventModel.cellType);
    });
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public onCellSelect(cell: CellModel): void {
    if (cell.coordinates) {
      this.cellSelectedEvent.emit(cell.coordinates);
    }
  }

  private buildGrid(shipCoordinates: Array<CoordinatesModel>): Array<RowModel> {
    const grid: Array<RowModel> = [
      new RowModel([
          new CellModel('', null, CellType.None), new CellModel('A', null, CellType.None), new CellModel('B', null, CellType.None),
          new CellModel('C', null, CellType.None), new CellModel('D', null, CellType.None), new CellModel('E', null, CellType.None),
          new CellModel('F', null, CellType.None), new CellModel('G', null, CellType.None), new CellModel('H', null, CellType.None),
          new CellModel('I', null, CellType.None), new CellModel('J', null, CellType.None)
        ], 0)
    ];

    for (let index = 0; index < 10; index++) {
      const rowNumber: number = index + 1;
      let coordinatesForRow: Array<CoordinatesModel> = null;
      if (shipCoordinates && shipCoordinates.length) {
        coordinatesForRow = shipCoordinates.filter(x => x.yPosition === rowNumber);
      }
      const row = this.buildRow(rowNumber, coordinatesForRow);
      grid.push(row);
    }

    return grid;
  }

  private buildRow(rowNumber: number, coordinatesForRow: Array<CoordinatesModel>): RowModel {
    const row = new RowModel([
      new CellModel(rowNumber.toString(), null, CellType.None)
    ], rowNumber);
    this.buildCells(rowNumber, row.cells, coordinatesForRow);

    return row;
  }

  private buildCells(rowNumber: number, cells: Array<CellModel>, coordinatesForRow: Array<CoordinatesModel>): void {
    for (let index = 0; index < 10; index++) {
      const columnNumber: number = index + 1;
      let cellType: CellType = CellType.Empty;
      if (coordinatesForRow) {
        const foundCoordinates: CoordinatesModel = coordinatesForRow.find(x => x.xPosition === columnNumber);
        if (foundCoordinates) {
          cellType = this.coordinateHelperService.getCellType(foundCoordinates.hit);
        }
      }
      const cell = new CellModel(null, new CoordinatesModel(columnNumber, rowNumber), cellType);
      cells.push(cell);
    }
  }

}

import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { CellModel } from '../models/cell-model';
import { CoordinatesModel } from '../models/coordinates-model';
import { RowModel } from '../models/row-model';

@Component({
  selector: 'app-game-grid',
  templateUrl: './game-grid.component.html',
})
export class GameGridComponent implements OnInit {
  public rows: Array<RowModel>;

  @Output() public cellSelectedEvent: EventEmitter<CoordinatesModel> = new EventEmitter<CoordinatesModel>();
  ngOnInit(): void {
    this.rows = this.buildGrid();
  }

  public onCellSelect(cell: CellModel): void {
    if (cell.coordinates) {
      this.cellSelectedEvent.emit(cell.coordinates);
    }
  }

  private buildGrid(): Array<RowModel> {
    const grid: Array<RowModel> = [
      new RowModel([
          new CellModel('', null), new CellModel('A', null), new CellModel('B', null), new CellModel('C', null), new CellModel('D', null),
          new CellModel('E', null), new CellModel('F', null), new CellModel('G', null), new CellModel('H', null),
          new CellModel('I', null), new CellModel('J', null)
        ])
    ];

    for (let index = 0; index < 10; index++) {
      const row = this.buildRow(index + 1);
      grid.push(row);
    }

    return grid;
  }

  private buildRow(rowNumber: number): RowModel {
    const row = new RowModel([
      new CellModel(rowNumber.toString(), null)
    ]);
    this.buildCells(rowNumber, row.cells);

    return row;
  }

  private buildCells(rowNumber: number, cells: Array<CellModel>): void {
    for (let index = 0; index < 10; index++) {
      const cell = new CellModel(null, new CoordinatesModel(index + 1, rowNumber));
      cells.push(cell);
    }
  }

}

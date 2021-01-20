import { Component, OnInit } from '@angular/core';
import { GameStoreService } from '../Services/game-store-service';
import { GameModel } from '../models/game-model';
import { SignalRService } from '../Services/signalR-service';
import { ShipModel } from '../models/ship-model';
import { IdNamePair } from '../models/id-name-pair';
import { ShipOrientationType } from '../models/enums/ship-orientation-type';
import { type } from "os";
import { CoordinatesModel } from '../models/coordinates-model';
import { UpdateShipPositionRequest } from '../models/requests/update-ship-position-request';
import { ShipApiService } from '../Services/ship-api-service';
import { finalize } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { GameApiService } from '../Services/game-api-services';
import { CoordinatesHelperService } from '../Services/coordinates-helper-service';
import { UpdatedShipEventModel } from '../models/updated-ship-event-model';

@Component({
  selector: 'app-get-setup',
  templateUrl: './game-setup.component.html',
})
export class GameSetupComponent implements OnInit {
  public isLoading: boolean = false;
  private _currentGame: GameModel;
  public get currentGame() { return this._currentGame; }
  public set currentGame(game: GameModel) {
    this._currentGame = game;
    if (this._currentGame) {
      this.shipSelectList = this.convertShipsToSelectList(this._currentGame.ships);
      this.shipCoordinates = this._currentGame.ships.reduce((coordinates, ship) => [...coordinates, ...ship.coordinates ], []);
    }
  }
  public selectedShipType: number;
  public shipSelectList: Array<IdNamePair>;
  public selectedOrientation: number;
  public selectedOrientationList: Array<IdNamePair> = [
    new IdNamePair (ShipOrientationType.HorizontalRight, 'Horizontal Right'),
    new IdNamePair (ShipOrientationType.VerticalDown, 'Vertical Down')
  ];
  public shipCoordinates: Array<CoordinatesModel> = [];
  constructor(private gameStoreService: GameStoreService, private signalRService: SignalRService, private shipApiService: ShipApiService,
    private route: ActivatedRoute, private gameApiService: GameApiService, private coordinatesHelperService: CoordinatesHelperService) {}
  ngOnInit(): void {
    this.currentGame = this.gameStoreService.currentGame;
    if (!this.currentGame) {
      const routeParams = this.route.snapshot.paramMap;
      const gameId: number = Number(routeParams.get('gameId'));
      const playerId: number = Number(routeParams.get('playerId'));
      this.isLoading = true;
      this.gameApiService.getGame(gameId, playerId)
      .pipe(
        finalize(() => this.isLoading = false)
      )
      .subscribe((data) => {
        if (data.success) {
          this.gameStoreService.currentGame = data.data;
          this.currentGame = data.data;
        } else {
          alert(data.message);
        }
      })
    }

    this.signalRService.addPlayerJoinedGameListener((joinedPlayer) => {
      if (this.currentGame.gameId === joinedPlayer.gameId) {
        alert(`Player ${joinedPlayer.playerId} has joined the game`);
      }
    });
  }

  public convertShipsToSelectList(ships: Array<ShipModel>): Array<IdNamePair> {
    return ships.map((ship) => new IdNamePair (ship.shipType, `${ship.name}(${ship.size})`));
  }

  public onCellSelectedEvent(coordinates: CoordinatesModel): void {
    if (this.selectedShipType === null || this.selectedShipType === undefined) {
      alert('You must select a ship');
      return;
    }
    if (this.selectedOrientation === null || this.selectedOrientation === undefined) {
      alert('You must select an orientation.');
      return;
    }

    const currentShipModel: ShipModel = this.currentGame.ships.find(x => x.shipType === parseInt(this.selectedShipType));

    const request = new UpdateShipPositionRequest(this.currentGame.gameId, this.currentGame.playerId, parseInt(this.selectedShipType.toString()),
                                                  coordinates, parseInt(this.selectedOrientation.toString()));
    this.isLoading = true;
    this.shipApiService.updateShipPosition(request)
    .pipe(
      finalize(() => this.isLoading = false)
    )
    .subscribe((data) => {
      if (data.success) {
        this.coordinatesHelperService.updatedShipEvent.emit(new UpdatedShipEventModel(currentShipModel, data.data));
        Object.assign(this.currentGame.ships.find(x => x.shipType === data.data.shipType), data.data);
      } else {
        alert(data.message);
      }
    });
  }
}

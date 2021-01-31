import { Component, OnDestroy, OnInit } from '@angular/core';
import { GameStoreService } from '../Services/game-store-service';
import { GameModel } from '../models/game-model';
import { SignalRService } from '../Services/signalR-service';
import { ShipModel } from '../models/ship-model';
import { IdNamePair } from '../models/id-name-pair';
import { ShipOrientationType } from '../models/enums/ship-orientation-type';
import { CoordinatesModel } from '../models/coordinates-model';
import { UpdateShipPositionRequest } from '../models/requests/update-ship-position-request';
import { ShipApiService } from '../Services/ship-api-service';
import { finalize, takeUntil } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { GameApiService } from '../Services/game-api-services';
import { CoordinatesHelperService } from '../Services/coordinates-helper-service';
import { UpdatedShipEventModel } from '../models/updated-ship-event-model';
import { GamePlayerRequest } from '../models/requests/game-player-request';
import { PlayerStatus } from '../models/enums/player-status';
import { PlayerFactory } from '../factories/player-factory';
import { IPlayer } from '../models/interfaces/player-interface';
import { SetGameStatusRequest } from '../models/requests/set-game-status-request';
import { GameStatus } from '../models/enums/game-status-type';
import { interval, Subject } from 'rxjs';

@Component({
  selector: 'app-get-setup',
  templateUrl: './game-setup.component.html',
})
export class GameSetupComponent implements OnInit, OnDestroy {
  public isLoading: boolean = false;
  public gameIsStarting: boolean = false;
  public secondsTillStart: number = 5;
  private _currentGame: GameModel;
  private thisPlayer: IPlayer;
  private otherPlayer: IPlayer;
  public get currentGame() { return this._currentGame; }
  public set currentGame(game: GameModel) {
    this._currentGame = game;
    if (this._currentGame) {
      this.shipSelectList = this.convertShipsToSelectList(this._currentGame.ships);
      this.shipCoordinates = this._currentGame.ships.reduce((coordinates, ship) => [...coordinates, ...ship.coordinates ], []);
      this.otherPlayer = this.playerFactory.getOtherPlayerFromGame(this._currentGame.playerId, this._currentGame);
      this.thisPlayer = this.playerFactory.getThisPlayerFromGame(this._currentGame.playerId, this._currentGame);
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
  private unsubscribe: Subject<null> = new Subject();
  constructor(private gameStoreService: GameStoreService, private signalRService: SignalRService, private shipApiService: ShipApiService,
    private route: ActivatedRoute, private gameApiService: GameApiService, private coordinatesHelperService: CoordinatesHelperService,
    private playerFactory: PlayerFactory, private router: Router) {}
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
      if (this.currentGame.gameId === joinedPlayer.gameId &&
        this.currentGame.playerId !== joinedPlayer.playerId) {
        this.otherPlayer.setPlayerStatus(this.currentGame, PlayerStatus.Preparing);
      }
    });

    this.signalRService.addPlayerIsReadyGameListener((updatedPlayer) => {
      if (this.currentGame.gameId === updatedPlayer.gameId ) {
        if (this.currentGame.playerId !== updatedPlayer.playerId) {
          this.otherPlayer.setPlayerStatus(this.currentGame, PlayerStatus.Ready);
        }
        this.currentGame.gameReadyToStart = updatedPlayer.bothPlayersReady;
      }
    });

    this.signalRService.addUpdatedGameStatusListener((updatedGame) => {
      if (this.currentGame.gameId === updatedGame.gameId) {
        this.gameIsStarting = true;
        this.gameStoreService.currentGame.currentPlayerIdTurn = updatedGame.currentPlayerIdTurn;
        interval(1000).pipe(
          takeUntil(this.unsubscribe))
        .subscribe(() => {
          this.secondsTillStart--;
          if (this.secondsTillStart === 0) {
            this.router.navigate([`/game-play-area/${this.currentGame.gameId}/${this.currentGame.playerId}`]);
          }
        });
      }
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
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

    const currentShipModel: ShipModel = this.currentGame.ships.find(x => x.shipType === parseInt(this.selectedShipType.toString()));

    const request = new UpdateShipPositionRequest(this.currentGame.gameId, this.currentGame.playerId,
                                                  parseInt(this.selectedShipType.toString()),
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


  public onReadyClick(): void {
    this.isLoading = true;
    this.gameApiService.setGameToReady(new GamePlayerRequest(this.currentGame.gameId, this.currentGame.playerId))
      .pipe(
        finalize(() => this.isLoading = false)
      ).subscribe((data) => {
        if (data.success) {
          this.thisPlayer.setPlayerStatus(this.currentGame, PlayerStatus.Ready);
        } else {
          alert(data.message);
        }
      });
  }

  public onGameStartClick(): void {
    this.isLoading = true;
    this.gameApiService.setGameStatus(new SetGameStatusRequest(this.currentGame.gameId, GameStatus.Started))
    .pipe(
      finalize(() => this.isLoading = false)
    ).subscribe((data) => {
      if (!data.success) {
        alert(data.message);
      }
    });
  }
}

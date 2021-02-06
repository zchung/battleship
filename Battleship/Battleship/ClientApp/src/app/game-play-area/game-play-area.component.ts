import { Component, EventEmitter, OnInit } from '@angular/core';
import { GameApiService } from '../Services/game-api-services';
import { GameStoreService } from '../Services/game-store-service';
import { GameModel } from '../models/game-model';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { CoordinatesModel } from '../models/coordinates-model';
import { AttackPlayerRequest } from '../models/requests/attack-player-request';
import { IPlayer } from '../models/interfaces/player-interface';
import { PlayerFactory } from '../factories/player-factory';
import { SignalRService } from '../Services/signalR-service';
import { CoordinatesHelperService } from '../Services/coordinates-helper-service';
import { SetNewCoordinatesEvent } from '../models/set-new-coordinates-event';
import { CellType } from '../models/enums/cell-type';

@Component({
  selector: 'app-game-play-area',
  templateUrl: './game-play-area.component.html',
}
)
export class GamePlayAreaComponent implements OnInit {
  public isLoading: boolean = false;
  private _currentGame: GameModel;
  public shipCoordinates: Array<CoordinatesModel> = [];
  public selectedCoordinates: Array<CoordinatesModel> = [];
  private thisPlayer: IPlayer;
  private otherPlayer: IPlayer;
  public get currentGame() { return this._currentGame; }
  public set currentGame(game: GameModel) {
    this._currentGame = game;
    if (this._currentGame) {
      this.shipCoordinates = this._currentGame.ships.reduce((coordinates, ship) => [...coordinates, ...ship.coordinates ], []);
      this.selectedCoordinates = this._currentGame.attemptedCoordinates;
      this.otherPlayer = this.playerFactory.getOtherPlayerFromGame(this._currentGame.playerId, this._currentGame);
      this.thisPlayer = this.playerFactory.getThisPlayerFromGame(this._currentGame.playerId, this._currentGame);
    }
  }

  public setCellTypeInOtherPlayerGrid: EventEmitter<SetNewCoordinatesEvent> = new EventEmitter<SetNewCoordinatesEvent>();
  public setCellTypeInThisPlayerGrid: EventEmitter<SetNewCoordinatesEvent> = new EventEmitter<SetNewCoordinatesEvent>();

  constructor(private gameApiService: GameApiService, private gameStoreService: GameStoreService,
              private route: ActivatedRoute, private playerFactory: PlayerFactory,
              private signalRService: SignalRService, private coordinatesHelperService: CoordinatesHelperService,
              private router: Router) {

  }

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
      ).subscribe((data) => {
        if (data.success) {
          this.currentGame = data.data;
        } else {
          alert(data.message);
        }
      });
    }

    this.signalRService.addSendAttackPlayerCoordinatesListener((data) => {
      if (data.success) {
        if (data.gameId === this.currentGame.gameId) {
          if (data.playerIdToAttack === this.thisPlayer.playerId) {
            this.currentGame.currentPlayerIdTurn = this.thisPlayer.playerId;
            this.setCellTypeInThisPlayerGrid.emit(
              new SetNewCoordinatesEvent(data.data, this.coordinatesHelperService.getCellType(data.data.hit))
            );
          }
          if (data.winnerOfGamePlayerId) {
            alert(`Game over winner is Player: ${data.winnerOfGamePlayerId}`);
            this.router.navigate(['/']);
          }
        }
      }
    });
  }

  public onCellSelectedEvent(coordinatesModel: CoordinatesModel): void {
    this.isLoading = true;
    this.gameApiService.attackPlayer(
      new AttackPlayerRequest(this.currentGame.gameId, this.thisPlayer.playerId, this.otherPlayer.playerId, coordinatesModel))
    .pipe(
      finalize(() => this.isLoading = false)
    ).subscribe((result) => {
      if (result.success) {
        this.setCellTypeInOtherPlayerGrid.emit(
          new SetNewCoordinatesEvent(result.data, this.coordinatesHelperService.getCellType(result.data.hit))
        );
        this.currentGame.currentPlayerIdTurn = this.otherPlayer.playerId;
      } else {
        alert(result.message);
      }
    })
  }

}

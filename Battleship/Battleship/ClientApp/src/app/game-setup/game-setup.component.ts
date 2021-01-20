import { Component, OnInit } from "@angular/core";
import { GameStoreService } from '../Services/game-store-service';
import { GameModel } from '../models/game-model';
import { SignalRService } from '../Services/signalR-service';
import { ShipModel } from '../models/ship-model';
import { IdNamePair } from '../models/id-name-pair';
import { ShipOrientationType } from '../models/enums/ship-orientation-type';
import { type } from "os";

@Component({
  selector: 'app-get-setup',
  templateUrl: './game-setup.component.html',
})
export class GameSetupComponent implements OnInit {
  public currentGame: GameModel;
  public selectedShipType: number;
  public shipSelectList: Array<IdNamePair>;
  public selectedOrientation: number;
  public selectedOrientationList: Array<IdNamePair> = [
    new IdNamePair (ShipOrientationType.HorizontalRight, 'Horizontal Right'),
    new IdNamePair (ShipOrientationType.VerticalDown, 'Vertical Down')
  ];
  constructor(private gameStoreService: GameStoreService, private signalRService: SignalRService) {}
  ngOnInit(): void {
    this.currentGame = this.gameStoreService.currentGame;
    this.shipSelectList = this.convertShipsToSelectList(this.currentGame.ships);
    this.signalRService.addPlayerJoinedGameListener((joinedPlayer) => {
      if (this.currentGame.gameId === joinedPlayer.gameId) {
        alert(`Player ${joinedPlayer.playerId} has joined the game`);
      }
    });
  }

  public convertShipsToSelectList(ships: Array<ShipModel>): Array<IdNamePair> {
    return ships.map((ship) => new IdNamePair (ship.shipType, `${ship.name}(${ship.size})`));
  }
}

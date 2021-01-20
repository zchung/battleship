import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameApiService } from '../Services/game-api-services';
import { CreateGameRequest } from '../models/requests/create-game-request';
import { GameModel } from '../models/game-model';
import { finalize } from 'rxjs/operators';
import { SignalRService } from '../Services/signalR-service';
import { GameStoreService } from '../Services/game-store-service';
import { GameListModel } from '../models/game-list-model';
import { JoinGameRequest } from '../models/requests/join-game-request';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{
  public isLoading: boolean = false;
  public isLoadingGames: boolean = false;
  public description: string = '';
  public gameList: Array<GameListModel>;

  constructor(private gameApiService: GameApiService, private router: Router,
              private gameStoreService: GameStoreService, private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.isLoadingGames = true;
    this.gameApiService.getActiveGames()
      .pipe(
        finalize(() => { this.isLoadingGames = false; })
    ).subscribe(result => {
      if (result.success) {
        this.gameList = result.data;
      }
    });

    this.signalRService.addNewGameListener((game: GameModel) => {
      this.gameList.push(game);
    });

    this.signalRService.addRemoveGameListener((game: GameModel) => {
      this.gameList = this.gameList.slice(this.gameList.indexOf(game), 1);
    });
  }

  public createGame(): void {
    this.isLoading = true;
    this.gameApiService.createGame(new CreateGameRequest(this.description))
    .pipe(
      finalize(() => { this.isLoadingGames = false; })
    ).subscribe((result) => {
      if (result.success) {
        this.gameStoreService.currentGame = result.data;
        this.router.navigate([`/game-setup/${result.data.gameId}/${result.data.playerId}`]);
      } else {
        alert(result.message);
      }
    });
  }

  public joinGame(gameId: number): void {
    this.isLoading = true;
    this.gameApiService.joinGame(new JoinGameRequest(gameId))
    .pipe(
      finalize(() => { this.isLoading = false; })
    ).subscribe((result) => {
      if (result.success) {
        this.gameStoreService.currentGame = result.data;
        this.router.navigate(['/game-setup']);
      } else {
        alert(result.message);
      }
    });
  }

}

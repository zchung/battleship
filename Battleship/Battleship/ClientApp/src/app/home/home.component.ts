import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameApiService } from '../Services/game-api-services';
import { CreateGameRequest } from '../models/requests/create-game-request';
import { Game } from '../models/game';
import { finalize } from 'rxjs/operators';
import { SignalRService } from '../Services/signalR-service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{
  public isLoading: boolean = false;
  public isLoadingGames: boolean = false;
  public description: string = '';
  public gameList: Array<Game>;

  constructor(private gameApiService: GameApiService, private router: Router, private signalRService: SignalRService) {}

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

    this.signalRService.addNewGameListener((game: Game) => {
      this.gameList.push(game);
    });
  }

  public createGame(): void {
    this.isLoading = true;
    this.gameApiService.createGame(new CreateGameRequest(this.description))
    .pipe(
      finalize(() => { this.isLoadingGames = false; })
    ).subscribe((result) => {
      if (result.success) {
        this.router.navigate(['/game-setup']);
      } else {
        alert(result.message);
      }
    });
  }

}

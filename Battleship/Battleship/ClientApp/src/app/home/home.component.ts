import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GameApiService } from '../Services/game-api-services';
import { CreateGameRequest } from '../models/requests/create-game-request';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public isLoading: boolean = false;
  public description: string = '';

  constructor(private gameApiService: GameApiService, private router: Router) {}

  public createGame(): void {
    this.isLoading = true;
    this.gameApiService.createGame(new CreateGameRequest(this.description)).subscribe((result) => {
      if (result.success) {
        this.router.navigate(['/game-setup']);
      } else {
        alert(result.message);
      }
    });
  }

}

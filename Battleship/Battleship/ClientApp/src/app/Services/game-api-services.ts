import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Result } from '../models/result-data';
import * as result from '../models/result';
import { CreateGameRequest } from '../models/requests/create-game-request';
import { GameModel } from '../models/game-model';
import { GameListModel } from '../models/game-list-model';
import { JoinGameRequest } from '../models/requests/join-game-request';
import { GamePlayerRequest } from '../models/requests/game-player-request';
import { SetGameStatusRequest } from '../models/requests/set-game-status-request';
import { UpdatedGame } from '../models/updated-game-model';
import { AttackPlayerRequest } from '../models/requests/attack-player-request';
import { CoordinatesModel } from '../models/coordinates-model';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class GameApiService {
  private baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = `game/${environment.baseUrl}`;
  }

  createGame(createGameRequest: CreateGameRequest): Observable<Result<GameModel>> {
    return this.http.post<Result<GameModel>>(`${environment.baseUrl}game/Create`, createGameRequest);
  }

  getActiveGames(): Observable<Result<Array<GameListModel>>> {
    return this.http.get<Result<Array<GameListModel>>>(`${environment.baseUrl}game/getactive`);
  }

  joinGame(joinGameRequest: JoinGameRequest): Observable<Result<GameModel>> {
    return this.http.post<Result<GameModel>>(`${environment.baseUrl}game/Join`, joinGameRequest);
  }

  getGame(gameId: number, playerId: number): Observable<Result<GameModel>> {
    return this.http.get<Result<GameModel>>(`${environment.baseUrl}game/get/${gameId}/${playerId}`);
  }

  setGameToReady(gamePlayerRequest: GamePlayerRequest): Observable<result.Result> {
    return this.http.post<result.Result>(`${environment.baseUrl}game/setPlayerToReady`, gamePlayerRequest);
  }

  setGameStatus(setGameStatusRequest: SetGameStatusRequest): Observable<result.Result> {
    return this.http.post<result.Result>(`${environment.baseUrl}game/setgamestatus`, setGameStatusRequest);
  }

  attackPlayer(attackPlayerRequest: AttackPlayerRequest): Observable<Result<CoordinatesModel>> {
    return this.http.post<Result<CoordinatesModel>>(`${environment.baseUrl}game/attackplayer`, attackPlayerRequest);
  }
}

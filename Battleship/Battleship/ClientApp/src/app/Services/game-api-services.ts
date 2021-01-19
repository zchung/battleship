import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Result } from '../models/result-data';
import { CreateGameRequest } from '../models/requests/create-game-request';
import { GameModel } from '../models/game-model';
import { GameListModel } from '../models/game-list-model';
import { JoinGameRequest } from '../models/requests/join-game-request';
@Injectable()
export class GameApiService {
  private baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = `${baseUrl}Game/`;
  }

  createGame(createGameRequest: CreateGameRequest): Observable<Result<GameModel>> {
    return this.http.post<Result<GameModel>>(`${this.baseUrl}Create`, createGameRequest);
  }

  getActiveGames(): Observable<Result<Array<GameListModel>>> {
    return this.http.get<Result<Array<GameListModel>>>(`${this.baseUrl}getactive`);
  }

  joinGame(joinGameRequest: JoinGameRequest): Observable<Result<GameModel>> {
    return this.http.post<Result<GameModel>>(`${this.baseUrl}Join`, joinGameRequest);
  }
}

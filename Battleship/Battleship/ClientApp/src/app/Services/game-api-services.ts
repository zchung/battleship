import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Result } from '../models/result-data';
import { CreateGameRequest } from '../models/requests/create-game-request';
import { Game } from '../models/game';
@Injectable()
export class GameApiService {
  private baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  createGame(createGameRequest: CreateGameRequest): Observable<Result<number>> {
    return this.http.post<Result<number>>(this.baseUrl + 'Game/Create', createGameRequest);
  }

  getActiveGames(): Observable<Result<Array<Game>>> {
    return this.http.get<Result<Array<Game>>>(this.baseUrl + 'Game/getactive');
  }
}

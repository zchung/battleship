import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GameModel } from '../models/game-model';
import { UpdateShipPositionRequest } from '../models/requests/update-ship-position-request';
import { Result } from '../models/result-data';

@Injectable({
  providedIn: 'root'
})
export class ShipApiService {
  private baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = `${baseUrl}Ship/`;
  }

  public updateShipPosition(request: UpdateShipPositionRequest): Observable<Result<GameModel>> {
    return this.http.post<Result<GameModel>>(`${this.baseUrl}updateshipposition`, request);
  }
}

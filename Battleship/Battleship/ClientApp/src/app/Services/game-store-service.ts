import { Injectable } from '@angular/core';
import { GameModel } from '../models/game-model';

@Injectable({
  providedIn: 'root'
})
export class GameStoreService {
  public currentGame: GameModel;
}

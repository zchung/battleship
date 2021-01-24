import { GameStatus } from '../enums/game-status-type'

export class SetGameStatusRequest {
  constructor(
  public gameId: number,
  public gameStatus: GameStatus) {}
}

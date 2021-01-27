import { CoordinatesModel } from './coordinates-model';
import { Result } from './result-data';
export class AttackingPlayerResult extends Result<CoordinatesModel> {
  gameId: number;
  playerIdAttacking: number;
  playerIdToAttack: number;
  winnerOfGamePlayerId?: number;
}

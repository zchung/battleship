import { CoordinatesModel } from '../coordinates-model';
export class AttackPlayerRequest {
  constructor(
  public gameId: number,
  public playerIdAttacking: number,
  public playerIdToAttack: number,
  public coordinatesViewModel: CoordinatesModel) {}
}

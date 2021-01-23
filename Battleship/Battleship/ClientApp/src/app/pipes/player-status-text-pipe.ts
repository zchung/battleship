import { Pipe, PipeTransform } from "@angular/core";
import { PlayerStatus } from '../models/enums/player-status';

@Pipe({
  name: 'playerStatusText'
})
export class PlayerStatusTextPipe implements PipeTransform {
  transform(playerStatus: PlayerStatus) {
    switch (playerStatus) {
      case PlayerStatus.Empty:
        return 'Waiting for another player';
      case PlayerStatus.Preparing:
        return 'Preparing';
      case PlayerStatus.Ready:
        return 'Ready';
      default:
        break;
    }
  }

}

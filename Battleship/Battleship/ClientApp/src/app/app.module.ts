import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { GameSetupComponent } from './game-setup/game-setup.component';
import { GameGridComponent } from './game-grid/game-grid.component';
import { CellStylePipe } from './pipes/cell-style-pipe';
import { PlayerStatusTextPipe } from './pipes/player-status-text-pipe';
import { GamePlayAreaComponent } from './game-play-area/game-play-area.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    GameSetupComponent,
    GameGridComponent,
    GamePlayAreaComponent,
    CellStylePipe,
    PlayerStatusTextPipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'game-setup/:gameId/:playerId', component: GameSetupComponent },
      { path: 'game-play-area/:gameId/:playerId', component: GamePlayAreaComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d170cbc1070b82f38af44bee67d54f56da5d2b4ca7bbf8e4c7451f8dcad7942d
// IndexVersion: 2
// --- END CODE INDEX META ---
import { SeaWolfSimComponent } from './seawolf-sim/seawolf-sim.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { SimComponent } from './sim/sim.component';

@NgModule({
   declarations: [
      AppComponent,
      SimComponent,
      SeaWolfSimComponent,
   ],
   imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
      HttpClientModule,
      FormsModule,
      RouterModule.forRoot([
         { path: '', component: SimComponent, pathMatch: 'full' },
         { path: 'seawolf', component: SeaWolfSimComponent, pathMatch: 'full' },
      ])
   ],
   providers: [],
   bootstrap: [AppComponent]
})
export class AppModule { }

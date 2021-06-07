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
      SimComponent
   ],
   imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
      HttpClientModule,
      FormsModule,
      RouterModule.forRoot([
         { path: '', component: SimComponent, pathMatch: 'full' },
      ])
   ],
   providers: [],
   bootstrap: [AppComponent]
})
export class AppModule { }

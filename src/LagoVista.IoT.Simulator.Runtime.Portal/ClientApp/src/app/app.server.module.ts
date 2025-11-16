// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 2001cea68694dda892e69f3801c4175e74b40159d17e91c4875fd875df691cdb
// IndexVersion: 2
// --- END CODE INDEX META ---
import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';
import { AppComponent } from './app.component';
import { AppModule } from './app.module';

@NgModule({
    imports: [AppModule, ServerModule, ModuleMapLoaderModule],
    bootstrap: [AppComponent]
})
export class AppServerModule { }

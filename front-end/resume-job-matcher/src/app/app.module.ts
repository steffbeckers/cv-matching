import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

// Modules
import { AppRoutingModule } from './app-routing.module';
import { AppStoreModule } from './store/store.module';

// Components
import { AppComponent } from './app.component';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, HttpClientModule, AppRoutingModule, AppStoreModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}

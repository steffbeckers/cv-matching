import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

// Modules
import { AppRoutingModule } from './app-routing.module';
import { AppStoreModule } from './store/store.module';

// Components
import { AppComponent } from './app.component';

// Services
import { AuthService } from './shared/services/auth.service';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, HttpClientModule, AppRoutingModule, AppStoreModule],
  providers: [AuthService],
  bootstrap: [AppComponent],
})
export class AppModule {}

import { Component, OnInit } from '@angular/core';

// RxJS
import { Observable } from 'rxjs';

// NgRx
import { select, Store } from '@ngrx/store';
import * as fromStore from './store';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  // Auth
  public auth$: Observable<any>;
  public authenticated$: Observable<boolean>;

  constructor(private store: Store<fromStore.AppState>) {}

  ngOnInit(): void {
    // Token from local storage
    const token: string = localStorage.getItem('token');
    if (token) {
      this.store.dispatch(new fromStore.LoadTokenFromLocalStorage(token));
    }

    // Auth
    this.auth$ = this.store.pipe(select(fromStore.getAuthState));
    this.authenticated$ = this.store.pipe(select(fromStore.getAuthenticated));
  }

  public login(): void {
    this.store.dispatch(
      new fromStore.Login({
        emailOrUsername: 'steff',
        password: 'Steff12345!',
        rememberMe: true,
      })
    );
  }

  public me(): void {
    this.store.dispatch(new fromStore.Me());
  }

  public logout(): void {
    this.store.dispatch(new fromStore.Logout());
  }
}

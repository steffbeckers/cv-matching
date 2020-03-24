import { Component, OnInit } from '@angular/core';

// NgRx
import { select, Store } from '@ngrx/store';
import * as fromStore from './store';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  public auth$: Observable<any>;

  constructor(private store: Store<fromStore.AppState>) {}

  ngOnInit(): void {
    this.auth$ = this.store.pipe(select(fromStore.getAuthState));
  }

  public login(): void {
    this.store.dispatch(
      new fromStore.Login({
        EmailOrUsername: 'steff',
        Password: 'Steff12345!',
        RememberMe: true,
      })
    );
  }
}

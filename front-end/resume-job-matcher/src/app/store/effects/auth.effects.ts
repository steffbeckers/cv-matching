import { Injectable } from '@angular/core';
import { Action, select, Store } from '@ngrx/store';
import { Actions, Effect, ofType } from '@ngrx/effects';

// RxJS
import { Observable, of, forkJoin } from 'rxjs';
import {
  catchError,
  debounceTime,
  map,
  mergeMap,
  exhaustMap,
  withLatestFrom,
} from 'rxjs/operators';

// NgRx
import { AppState } from '../reducers';
import * as AuthActions from '../actions/auth.actions';

// Models
import { User, Authenticated } from '../../shared/models/auth';

// Services
import { AuthService } from 'src/app/shared/services/auth.service';

@Injectable()
export class AuthEffect {
  constructor(private actions: Actions, private authService: AuthService) {}

  @Effect()
  login: Observable<AuthActions.AuthActionsAll> = this.actions.pipe(
    ofType<AuthActions.Login>(AuthActions.LOGIN),
    exhaustMap((action) => {
      return this.authService.login(action.payload).pipe(
        map((authenticated: Authenticated) => {
          return new AuthActions.LoginSuccess(authenticated);
        }),
        catchError((error) => of(new AuthActions.LoginFailed(error)))
      );
    })
  );
}

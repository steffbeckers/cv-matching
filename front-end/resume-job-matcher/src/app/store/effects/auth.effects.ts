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
  tap,
} from 'rxjs/operators';

// NgRx
import { AppState } from '../reducers';
import * as AuthActions from '../actions/auth.actions';

// Models
import { User, Authenticated } from '../../shared/models/auth';

// Services
import { AuthService } from 'src/app/shared/services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthEffect {
  constructor(
    private actions: Actions,
    private authService: AuthService,
    private router: Router
  ) {}

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

  @Effect({ dispatch: false })
  LoginSuccess: Observable<any> = this.actions.pipe(
    ofType(AuthActions.LOGIN_SUCCESS),
    tap((action) => {
      localStorage.setItem('token', action.payload.Token);
      this.router.navigateByUrl('/');
    })
  );

  @Effect({ dispatch: false })
  LogInFailure: Observable<any> = this.actions.pipe(
    ofType(AuthActions.LOGIN_FAILED)
  );
}

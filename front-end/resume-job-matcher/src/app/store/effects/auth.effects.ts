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
  switchMap,
} from 'rxjs/operators';

// NgRx
import { AppState, getAuthState } from 'src/app/store/reducers';
import * as AuthActions from 'src/app/store/actions/auth.actions';

// Models
import {
  User,
  Authenticated,
  Login as Credentials,
} from '../../shared/models/auth';

// Services
import { AuthService } from 'src/app/shared/services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthEffects {
  constructor(
    private store: Store<AppState>,
    private actions: Actions,
    private authService: AuthService,
    private router: Router
  ) {}

  // Load token from local storage
  @Effect()
  LoadTokenFromLocalStorage: Observable<
    AuthActions.AuthActionsAll
  > = this.actions.pipe(
    ofType<AuthActions.SetToken>(AuthActions.SET_TOKEN),
    withLatestFrom(this.store.pipe(select(getAuthState))),
    mergeMap(() => [new AuthActions.Me()])
  );

  // Login
  @Effect()
  Login: Observable<AuthActions.AuthActionsAll> = this.actions.pipe(
    ofType<AuthActions.Login>(AuthActions.LOGIN),
    map((action) => action.payload),
    switchMap((payload: Credentials) => {
      return this.authService.login(payload).pipe(
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
      this.authService.setToken(action.payload.token);
      this.router.navigateByUrl('/');
    })
  );

  @Effect({ dispatch: false })
  LoginFailed: Observable<any> = this.actions.pipe(
    ofType(AuthActions.LOGIN_FAILED),
    tap((error: any) => {})
  );

  // Me
  @Effect()
  Me: Observable<AuthActions.AuthActionsAll> = this.actions.pipe(
    ofType<AuthActions.Me>(AuthActions.ME),
    switchMap(() => {
      return this.authService.me().pipe(
        map((user: User) => {
          return new AuthActions.MeSuccess(user);
        }),
        catchError((error) => of(new AuthActions.MeFailed(error)))
      );
    })
  );

  @Effect({ dispatch: false })
  MeSuccess: Observable<any> = this.actions.pipe(
    ofType(AuthActions.ME_SUCCESS),
    tap((action) => {})
  );

  @Effect({ dispatch: false })
  MeFailed: Observable<any> = this.actions.pipe(
    ofType(AuthActions.ME_FAILED),
    tap((error: any) => {})
  );

  // Logout
  @Effect()
  Logout: Observable<AuthActions.AuthActionsAll> = this.actions.pipe(
    ofType<AuthActions.Logout>(AuthActions.LOGOUT),
    switchMap(() => {
      return this.authService.logout().pipe(
        map(() => {
          return new AuthActions.LogoutSuccess();
        }),
        catchError((error) => of(new AuthActions.LogoutFailed(error)))
      );
    })
  );

  @Effect({ dispatch: false })
  LogoutSuccess: Observable<User> = this.actions.pipe(
    ofType(AuthActions.LOGOUT_SUCCESS),
    tap(() => {
      localStorage.removeItem('token');
    })
  );

  @Effect({ dispatch: false })
  LogoutFailed: Observable<any> = this.actions.pipe(
    ofType(AuthActions.LOGOUT_FAILED),
    tap((error: any) => {})
  );
}

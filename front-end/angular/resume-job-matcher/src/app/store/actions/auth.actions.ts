import { Action } from '@ngrx/store';
import {
  Authenticated,
  User,
  Login as Credentials,
} from './../../shared/models/auth';

export const SET_TOKEN = '[AUTH] SET TOKEN';

export const LOGIN = '[AUTH] LOGIN';
export const LOGIN_SUCCESS = '[AUTH] LOGIN SUCCESS';
export const LOGIN_FAILED = '[AUTH] LOGIN FAILED';

export const ME = '[AUTH] ME';
export const ME_SUCCESS = '[AUTH] ME SUCCESS';
export const ME_FAILED = '[AUTH] ME FAILED';

export const LOGOUT = '[AUTH] LOGOUT';
export const LOGOUT_SUCCESS = '[AUTH] LOGOUT SUCCESS';
export const LOGOUT_FAILED = '[AUTH] LOGOUT FAILED';

// Set token
export class SetToken implements Action {
  readonly type = SET_TOKEN;

  constructor(public payload: string) {}
}

// Login
export class Login implements Action {
  readonly type = LOGIN;

  constructor(public payload: Credentials) {}
}

export class LoginSuccess implements Action {
  readonly type = LOGIN_SUCCESS;

  constructor(public payload: Authenticated) {}
}

export class LoginFailed implements Action {
  readonly type = LOGIN_FAILED;

  constructor(public payload: any) {}
}

// Me
export class Me implements Action {
  readonly type = ME;
}

export class MeSuccess implements Action {
  readonly type = ME_SUCCESS;

  constructor(public payload: User) {}
}

export class MeFailed implements Action {
  readonly type = ME_FAILED;

  constructor(public payload: any) {}
}

// Logout
export class Logout implements Action {
  readonly type = LOGOUT;
}

export class LogoutSuccess implements Action {
  readonly type = LOGOUT_SUCCESS;
}

export class LogoutFailed implements Action {
  readonly type = LOGOUT_FAILED;

  constructor(public payload: any) {}
}

export type AuthActionsAll =
  | SetToken
  | Login
  | LoginSuccess
  | LoginFailed
  | Me
  | MeSuccess
  | MeFailed
  | Logout
  | LogoutSuccess
  | LogoutFailed;

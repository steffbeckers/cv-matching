import { Action } from '@ngrx/store';

export const LOGIN = '[AUTH] LOGIN';
export const LOGIN_SUCCESS = '[AUTH] LOGIN SUCCESS';
export const LOGIN_FAILED = '[AUTH] LOGIN FAILED';

export const ME = '[AUTH] ME';
export const ME_SUCCESS = '[AUTH] ME SUCCESS';
export const ME_FAILED = '[AUTH] ME FAILED';

// Login
export class Login implements Action {
  readonly type = LOGIN;

  constructor() {}
}

export class LoginSuccess implements Action {
  readonly type = LOGIN_SUCCESS;

  constructor(public payload: any) {}
}

export class LoginFailed implements Action {
  readonly type = LOGIN_FAILED;

  constructor(public payload: any) {}
}

// Me
export class Me implements Action {
  readonly type = ME;

  constructor() {}
}

export class MeSuccess implements Action {
  readonly type = ME_SUCCESS;

  constructor(public payload: any) {}
}

export class MeFailed implements Action {
  readonly type = ME_FAILED;

  constructor(public payload: any) {}
}

export type AuthActionsAll = Login | LoginSuccess | LoginFailed | Me | MeSuccess | MeFailed;

import * as AuthActions from '../actions/auth.actions';
import { User } from '../../shared/models/auth';

export interface AuthState {
  authenticated: boolean;
  user: User;
  token: string;
  rememberMe: boolean;
  loading: boolean;
  loaded: boolean;
}

export const AuthInitialState: AuthState = {
  authenticated: false,
  user: null,
  token: null,
  rememberMe: true,
  loading: false,
  loaded: false,
};

export function AuthReducer(
  state = AuthInitialState,
  action: AuthActions.AuthActionsAll
): AuthState {
  switch (action.type) {
    case AuthActions.LOGIN: {
      return {
        ...state,
        loading: true,
      };
    }

    case AuthActions.LOGIN_SUCCESS: {
      return {
        ...state,
        user: action.payload.User,
        token: action.payload.Token,
        rememberMe: action.payload.RememberMe,
        loading: false,
        loaded: true,
      };
    }

    case AuthActions.LOGIN_FAILED: {
      return {
        ...state,
        loading: false,
        loaded: false,
      };
    }

    default:
      return state;
  }
}

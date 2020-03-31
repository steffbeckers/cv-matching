import * as AuthActions from 'src/app/store/actions/auth.actions';
import { User } from '../../shared/models/auth';

export interface AuthState {
  authenticated: boolean;
  user: User;
  token: string;
  rememberMe: boolean;
  loading: boolean;
  errors: any;
}

export const AuthInitialState: AuthState = {
  authenticated: false,
  user: null,
  token: null,
  rememberMe: true,
  loading: false,
  errors: null,
};

export function AuthReducer(
  state = AuthInitialState,
  action: AuthActions.AuthActionsAll
): AuthState {
  switch (action.type) {
    // Set token
    case AuthActions.SET_TOKEN: {
      return {
        ...state,
        token: action.payload,
      };
    }

    // Login
    case AuthActions.LOGIN: {
      return {
        ...state,
        loading: true,
      };
    }

    case AuthActions.LOGIN_SUCCESS: {
      return {
        ...state,
        authenticated: true,
        user: action.payload.user,
        token: action.payload.token,
        rememberMe: action.payload.rememberMe,
        loading: false,
      };
    }

    case AuthActions.LOGIN_FAILED: {
      return {
        ...state,
        loading: false,
      };
    }

    // Me
    case AuthActions.ME: {
      return {
        ...state,
        loading: true,
      };
    }

    case AuthActions.ME_SUCCESS: {
      return {
        ...state,
        authenticated: true,
        user: action.payload,
        loading: false,
      };
    }

    case AuthActions.ME_FAILED: {
      return {
        ...state,
        loading: false,
      };
    }

    // Logout
    case AuthActions.LOGOUT: {
      return {
        ...state,
        loading: true,
      };
    }

    case AuthActions.LOGOUT_SUCCESS: {
      return {
        ...AuthInitialState,
      };
    }

    case AuthActions.LOGOUT_FAILED: {
      return {
        ...state,
        loading: false,
      };
    }

    // Default
    default:
      return state;
  }
}

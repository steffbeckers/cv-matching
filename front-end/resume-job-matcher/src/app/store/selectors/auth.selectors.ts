import { createSelector } from '@ngrx/store';
import { getAppState, AppState, AuthState } from '../reducers';

export const getAuthState = createSelector(
  getAppState,
  (state: AppState) => state.auth
);

export const getUser = createSelector(
  getAuthState,
  (state: AuthState) => state.user
);
export const getToken = createSelector(
  getAuthState,
  (state: AuthState) => state.token
);
export const getAuthenticated = createSelector(
  getAuthState,
  (state: AuthState) => state.authenticated
);

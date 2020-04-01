import { createSelector } from '@ngrx/store';
import { AuthState, getAuthState } from 'src/app/store/reducers';

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

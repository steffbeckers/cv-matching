import { ActionReducerMap, createFeatureSelector } from '@ngrx/store';
import { AuthReducer, AuthState } from './auth.reducer';

export interface AppState {
  auth: AuthState;
}

export const reducers: ActionReducerMap<AppState> = {
  auth: AuthReducer,
};

export const getAuthState = createFeatureSelector<AuthState>('auth');

export * from './auth.reducer';

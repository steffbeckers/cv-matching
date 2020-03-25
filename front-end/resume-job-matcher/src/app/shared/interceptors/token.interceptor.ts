import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';

// RxJS
import { Observable } from 'rxjs';

// NgRx
import { select, Store } from '@ngrx/store';
import * as fromStore from '../../store';

@Injectable({ providedIn: 'root' })
export class TokenInterceptor implements HttpInterceptor {
  private auth: fromStore.AuthState;

  constructor(private store: Store<fromStore.AppState>) {
    this.store
      .pipe(select(fromStore.getAuthState))
      .subscribe((auth: fromStore.AuthState) => {
        this.auth = auth;
      });
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // If user is authenticated, set token on requests
    if (this.auth.authenticated) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${this.auth.token}`,
        },
      });
    }

    return next.handle(req);
  }
}

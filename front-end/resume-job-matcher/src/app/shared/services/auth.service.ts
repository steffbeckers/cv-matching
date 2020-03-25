import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

// RxJs
import { Observable } from 'rxjs';

// NgRx
// import { select, Store } from '@ngrx/store';
// import * as fromStore from '../../store';

// Models
import { Login as Credentials, Authenticated, User } from '../models/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  // private token: string;

  constructor(
    private http: HttpClient
  ) // private store: Store<fromStore.AppState>
  {
    // this.store.pipe(select(fromStore.getToken)).subscribe((token: string) => {
    //   this.token = token;
    // });
  }

  public login(credentials: Credentials): Observable<Authenticated> {
    return this.http.post<Authenticated>(
      environment.api + '/auth/login',
      credentials
    );
  }

  public me(): Observable<User> {
    return this.http.get<User>(environment.api + '/auth/me');
  }

  public logout(): Observable<void> {
    return this.http.get<void>(environment.api + '/auth/logout');
  }
}

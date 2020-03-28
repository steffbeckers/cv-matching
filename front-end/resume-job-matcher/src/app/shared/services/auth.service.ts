import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

// RxJs
import { Observable } from 'rxjs';

// Models
import { Login as Credentials, Authenticated, User } from '../models/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) {}

  public getToken(): string {
    // Using local storage

    // TODO: Validate token if exists
    return localStorage.getItem('token');
  }

  public setToken(token: string): void {
    // Using local storage

    // TODO: Validate token
    localStorage.setItem('token', token);
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

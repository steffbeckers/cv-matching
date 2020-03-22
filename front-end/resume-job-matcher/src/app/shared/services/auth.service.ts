import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

// RxJs
import { Observable } from 'rxjs';

// Models
import { Login as Credentials, Authenticated } from '../models/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private httpClient: HttpClient) {}

  public login(credentials: Credentials): Observable<Authenticated> {
    return this.httpClient.post<Authenticated>(environment.api + '/auth/login', credentials);
  }
}

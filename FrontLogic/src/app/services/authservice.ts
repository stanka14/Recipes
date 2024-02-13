import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private token: string | null = null;
  private loginUrl = 'http://localhost:63855/api/user/login';

  constructor(private http: HttpClient) {

  }

  setToken(token: string) {
    this.token = token;
  }

  getToken(): string | null {
    return this.token;
  }

  addTokenToHeaders(headers: HttpHeaders): HttpHeaders {
    const token = this.getToken();
    if (token) {
      return headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  }


  login(data: any): Observable<any> {
    const login = {
        email: data.email,
        password: data.password
    }
    return this.http.post(this.loginUrl, login);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('isLoggedIn');
    this.token = null;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

}

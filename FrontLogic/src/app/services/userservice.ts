import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private url = 'http://localhost:63855/api/user';
  private headersWithoutToken: HttpHeaders;

  constructor(private http: HttpClient) {
    this.headersWithoutToken = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
  }

  private getHeadersWithToken(): HttpHeaders {
    const token = localStorage.getItem('token');
    if (token) {
      return new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + token
      });
    } else {
      return this.headersWithoutToken;
    }
  }

  register(userDto: any): Observable<any> {
    return this.http.post<any>(this.url + '/register', userDto, { headers: this.getHeadersWithToken() });
  }

  loadCurrentUser(): Observable<any> {
    return this.http.get<User>(this.url + '/current', { headers: this.getHeadersWithToken() });
  }

  getAllUsers(): Observable<any> {
    return this.http.get<User>(this.url + '/getall', { headers: this.getHeadersWithToken() });
  }

  getUserWithId(userId: string): Observable<any> {
    return this.http.get<any>(this.url + '/getuser?userId=' + userId, { headers: this.getHeadersWithToken() });
  }

  getAllNotifications(userId: string): Observable<any> {
    return this.http.get<any>(this.url + '/notifications?userId=' + userId, { headers: this.getHeadersWithToken() });
  }

  makeAsRead(notificationId: string): Observable<any> {
    return this.http.get<any>(this.url + '/makeasread?notificationId=' + notificationId, { headers: this.getHeadersWithToken() });
  }

  deleteNotification(notificationId: string): Observable<any> {
    return this.http.get<any>(this.url + '/deletenotification?notificationId=' + notificationId, { headers: this.getHeadersWithToken() });
  }

  makeAllAsRead(userId: string): Observable<any> {
    return this.http.get<any>(this.url + '/makeallasread?userId=' + userId, { headers: this.getHeadersWithToken() });
  }

  updateUser(data: any): Observable<any> {
    return this.http.post<any>(this.url + '/updateuser', data, { headers: this.getHeadersWithToken() });
  }

  changePassword(data: any): Observable<any> {
    return this.http.post<any>(this.url + '/changepassword', data, { headers: this.getHeadersWithToken() });
  }
}

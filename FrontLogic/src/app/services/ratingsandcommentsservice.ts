import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RatingsAndCommentsService {
  private recipesUrl = 'http://localhost:63855/api/recipes';
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

  private createRequestOptions(): object {
    return { headers: this.getHeadersWithToken() };
  }

  addRating(ratingDto: any): Observable<any> {
    return this.http.post<any>(this.recipesUrl + '/ratings', ratingDto, this.createRequestOptions());
  }

  addComment(commentDto: any): Observable<any> {
    return this.http.post<any>(this.recipesUrl + '/comments', commentDto, this.createRequestOptions());
  }

  getallRecipeRatings(loggedInUser: any, recipeId: any): Observable<any> {
    return this.http.get<any[]>(`${this.recipesUrl}/ratings/${loggedInUser}/${recipeId}`, this.createRequestOptions());
  }

  getallRecipeComments(loggedInUser: any, recipeId: any): Observable<any> {
    return this.http.get<any[]>(`${this.recipesUrl}/comments/${loggedInUser}/${recipeId}`, this.createRequestOptions());
  }

  deleteRating(id: any): Observable<any> {
    return this.http.delete<any>(this.recipesUrl + '/ratings/' + id, this.createRequestOptions());
  }

  deleteComment(id: any): Observable<any> {
    return this.http.delete<any>(this.recipesUrl + '/comments/' + id, this.createRequestOptions());
  }
}

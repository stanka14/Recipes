import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
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

  addRecipe(recipeDto: any): Observable<any> {
    return this.http.post<any>(this.recipesUrl + '/add', recipeDto, this.createRequestOptions());
  }

  getallUserRecipes(userId: any): Observable<any> {
    return this.http.get<any>(this.recipesUrl + '/userrecepies?userId=' + userId, this.createRequestOptions());
  }

  getallRecipes(userId: string): Observable<any> {
    return this.http.get<any>(`${this.recipesUrl}/${userId}`, this.createRequestOptions());
  }
  
  getLatestRecipes(count: any): Observable<any> {
    return this.http.get<any>(this.recipesUrl + '/latest?count=' + count, this.createRequestOptions());
  }

  getTopRatedRecipes(count: any): Observable<any> {
    return this.http.get<any>(this.recipesUrl + '/toprated?count=' + count, this.createRequestOptions());
  }

  getallUserFavouriteRecipes(userId: any): Observable<any> {
    return this.http.get<any>(this.recipesUrl + '/userfavouriterecepies?userId=' + userId, this.createRequestOptions());
  }

  getSelectedRecipe(id: any): Observable<any> {
    return this.http.get<any>(this.recipesUrl + '/getrecipe?id=' + id, this.createRequestOptions());
  }

  deleteSelectedRecipe(id: any): Observable<any> {
    return this.http.delete<any>(this.recipesUrl + '/' + id, this.createRequestOptions());
  }

  updateSelectedRecipe(recipeDto: any): Observable<any> {
    return this.http.put<any>(this.recipesUrl, recipeDto, this.createRequestOptions());
  }
  
  addRecipeToUserFavourites(userId: string, recipeId: string): Observable<any> {
    return this.http.get<any[]>(`${this.recipesUrl}/addtofavourites/${userId}/${recipeId}`, this.createRequestOptions());
  }

  removeRecipeFromUserFavourites(userId: string, recipeId: string): Observable<any> {
    return this.http.get<any[]>(`${this.recipesUrl}/removefromfavourites/${userId}/${recipeId}`, this.createRequestOptions());
  }
}

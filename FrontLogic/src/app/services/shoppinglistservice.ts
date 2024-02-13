import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ShoppingListService {
  private shoppingListUrl = 'http://localhost:63855/api/shopping';
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

  getUserShoppingList(): Observable<any[]> {
    return this.http.get<any[]>(`${this.shoppingListUrl}/shoppinglist`, this.createRequestOptions());
  }

  addIngredientToShoppingList(ingredient: any): Observable<any> {
    return this.http.post<any>(`${this.shoppingListUrl}/addIngredient`, ingredient, this.createRequestOptions());
  }

    removeIngredientFromShoppingList(ingredientId: any): Observable<any> {
        return this.http.delete<any>(this.shoppingListUrl + '/' + ingredientId, this.createRequestOptions());
      }
    
    }

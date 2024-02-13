import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ShoppingListService } from '../services/shoppinglistservice';
import { ShoppingList } from '../models/shoppingList';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.scss']
})
export class ShoppingListComponent implements OnInit {
  shoppingList: any[] = [];
  newIngredientForm: FormGroup;

  constructor(private fb: FormBuilder, private shoppingListService: ShoppingListService, private snackBar: MatSnackBar) {
    this.newIngredientForm = this.fb.group({
      name: ['', [Validators.required]],
      quantity: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.loadShoppingList();
  }

  loadShoppingList() {
    this.shoppingListService.getUserShoppingList().subscribe({
      next: (response: any) => {
        var shoppingList = response as ShoppingList;
        this.shoppingList = shoppingList?.ingredients || [];
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  addIngredient() {
    if (this.newIngredientForm.valid) {
      const { name, quantity } = this.newIngredientForm.value;
      const newIngredient = { name, quantity, id: '' };

      this.shoppingListService.addIngredientToShoppingList(newIngredient).subscribe({
        next: () => {
          this.newIngredientForm.reset({ quantity: 1 });
          this.loadShoppingList();
        },
        error: (error) => {
          this.processError(error);
        }
      });
    } else {
        this.alertError('Please enter a valid ingredient name and quantity.');
      
      this.markFormGroupTouched(this.newIngredientForm);
    }
  }

  removeIngredient(ingredientId: string) {
    this.shoppingListService.removeIngredientFromShoppingList(ingredientId).subscribe({
      next: () => {
        this.loadShoppingList();
        this.alertSuccess('Ingredient removed from the shopping list.');
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  private processError(error: any) {
    this.alertError('Error occurred while processing your request.');
  }

  private alertSuccess(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }

  private alertError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: 'error-snackbar'
    });
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}

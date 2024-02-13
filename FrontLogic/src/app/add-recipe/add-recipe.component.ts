import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RecipeService } from '../services/recipeservice';
import { RecipeCategoryEnum } from '../models/recipe';
import { UserService } from '../services/userservice';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.scss']
})
export class AddRecipeComponent implements OnInit {
  newRecipeForm: FormGroup;
  messageSuccess: boolean = false;
  selectedImage: File | null = null;
  message: string = '';
  userId: string = '';
  recipeCategories = Object.values(RecipeCategoryEnum).filter(item => typeof item === 'string');

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private recipeservice: RecipeService,
    private userservice: UserService,
    private snackBar: MatSnackBar
  ) {
    this.newRecipeForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      imageUrl: new FormControl(''),
      category: new FormControl('', Validators.required),
      ingredients: this.fb.array([])  
    });

    this.userservice.loadCurrentUser().subscribe({
      next: (response) => {
        this.userId = response.id;
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  ngOnInit(): void {
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    this.selectedImage = file;
  }

  addRecipe() {
    if (this.newRecipeForm.valid) {
      this.message = '';
      const recipeToAdd = {
        name: this.newRecipeForm.value.name,
        description: this.newRecipeForm.value.description,
        imageUrl: this.newRecipeForm.value.imageUrl,
        userId: this.userId,
        ingredients: this.newRecipeForm.value.ingredients || [],  
        id: ''
      };

      this.recipeservice.addRecipe(recipeToAdd).subscribe({
        next: (response) => {
          this.selectedImage = null;
          this.newRecipeForm.reset();
          this.newRecipeForm.controls.name.setErrors(null);
          this.newRecipeForm.controls.description.setErrors(null);
          this.newRecipeForm.controls.category.setErrors(null);
          this.newRecipeForm.controls.ingredients.setErrors(null);
          const ingredientsArray = this.newRecipeForm.get('ingredients') as FormArray;
          ingredientsArray.clear();
          this.newRecipeForm.markAsPristine();
          this.newRecipeForm.markAsUntouched();

          this.messageSuccess = true;
          this.message = 'Recipe is added.';
          this.route.data.subscribe(() => {});
          this.alertError('Recipe added successfully.');
        },
        error: (error) => {
          this.processError(error);
        }
      });
    } else {
      this.markFormGroupTouched(this.newRecipeForm);
    }
  }

  addIngredient() {
    const ingredients = this.newRecipeForm.get('ingredients') as FormArray;
    ingredients.push(this.fb.group({
      name: ['', Validators.required],
      quantity: ['', Validators.required],
      id: ['']
    }));
  }

  removeIngredient(index: number) {
    const ingredients = this.newRecipeForm.get('ingredients') as FormArray;
    ingredients.removeAt(index);
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  processError(error)
  {
    if(error.error.errors){
      var errors = error.error.errors;
      Object.keys(errors).forEach(key => {
        this.alertError(`Key: ${key}, Value: ${errors[key][0]}`);
      });   
    }
    else
    {
      this.alertError(error.error);
    }   
  }

  alertError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }
}

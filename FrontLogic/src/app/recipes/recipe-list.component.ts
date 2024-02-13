import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RecipeService } from '../services/recipeservice';
import { Recipe, RecipeCategoryEnum } from '../models/recipe';
import { Ingredient } from '../models/ingredient';
import { UserService } from '../services/userservice';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.scss']
})
export class RecipeListComponent implements OnInit {
  searchTerm: string = '';
  filteredRecipes: Recipe[] = [];
  recipes: Recipe[] = [];
  recipeCategories = ['All', ...Object.values(RecipeCategoryEnum).filter(item => typeof item === 'string')];
  selectedCategory: RecipeCategoryEnum | 'All' = 'All';
  userId: string = '';

  constructor(
    private userservice: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private recipeservice: RecipeService,
    private snackBar: MatSnackBar  
  ) {
    this.route.queryParams.subscribe(params => {
      if (params['updated']) {
        this.loadRecipes();
      }
    });

    this.loadRecipes();
  }

  ngOnInit(): void {}

  loadRecipes(): void {
    this.userservice.loadCurrentUser().subscribe({
      next: (response) => {
        this.userId = response.id;
        this.recipeservice.getallUserRecipes(this.userId).subscribe({
          next: (response: Recipe[]) => {
            this.recipes = response.map(recipe => ({
              name: recipe.name,
              createdAt: recipe.createdAt,
              id: recipe.id,
              userId: recipe.userId,
              description: recipe.description,
              favourite: recipe.favourite,
              category: recipe.category,
              imageUrl: '/assets/images/' + recipe.imageUrl,
              ingredients: recipe.ingredients || [] 
            }));
            this.filteredRecipes = this.recipes;
          },
          error: (error) => {
            this.processError(error);
          }    
        });
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  onSearch() {
    this.filteredRecipes = this.recipes.filter(recipe =>
      recipe.name.toLowerCase().includes(this.searchTerm.toLowerCase()));

    if (this.selectedCategory && this.selectedCategory !== 'All') {
      this.filteredRecipes = this.filteredRecipes.filter(recipe =>
        recipe.category === this.selectedCategory);
    }
  }

  showRecipeDetails(recipeId: string) {
    this.router.navigate(['/auth/recipe-details', recipeId]);
  }

  toggleFavorite(recipeId: string, event: Event) {
    event.stopPropagation();
    const foundRecipe = this.recipes.find((recipe) => recipe.id === recipeId);

    if (foundRecipe && foundRecipe.favourite) {
      this.removeFromFavourites(recipeId);
    } else {
      this.addToFavourites(recipeId);
    }
  }

  addToFavourites(recipeId: string) {
    this.recipeservice.addRecipeToUserFavourites(this.userId, recipeId).subscribe({
      next: (response: Recipe) => {
        const recipeIndex = this.recipes.findIndex((recipe) => recipe.id === recipeId);

        if (recipeIndex !== -1) {
          this.recipes[recipeIndex].favourite = response.favourite;
        }
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  removeFromFavourites(recipeId: string) {
    this.recipeservice.removeRecipeFromUserFavourites(this.userId, recipeId).subscribe({
      next: (response: Recipe) => {
        const recipeIndex = this.recipes.findIndex((recipe) => recipe.id === recipeId);

        if (recipeIndex !== -1) {
          this.recipes[recipeIndex].favourite = response.favourite;
        }
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  processError(error) {
    if (error.error.errors) {
      var errors = error.error.errors;
      Object.keys(errors).forEach(key => {
        this.alertError(`Key: ${key}, Value: ${errors[key][0]}`);
      });   
    } else {
      this.alertError(error.error);
    }   
  }

  alertError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
    console.error(message);
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, RouteConfigLoadEnd, Router } from '@angular/router';
import { RecipeService } from '../services/recipeservice';
import { Recipe, RecipeCategoryEnum } from '../models/recipe';
import { UserService } from '../services/userservice';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-dashboard-crm',
    templateUrl: './dashboard-crm.component.html',
    styleUrls: ['./dashboard-crm.component.scss']
})

export class DashboardCrmComponent implements OnInit {

    recipeCategories = ['All', ...Object.values(RecipeCategoryEnum).filter(item => typeof item === 'string')];
    selectedCategory: RecipeCategoryEnum | 'All' = 'All';
    searchTerm: string = ''; 
    filteredRecipes: Recipe[] = []; 
    recipes: Recipe[] = [];
    userId: string | null = null;
    constructor(private userservice: UserService, private route: ActivatedRoute, private router: Router, private recipeservice: RecipeService, private snackBar: MatSnackBar) {
      this.route.queryParams.subscribe(params => {
        if (params['updated']) {
          this.loadRecipes();
        }
      });
     }
  
     loadRecipes(): void
     {
       this.recipeservice.getallUserFavouriteRecipes(this.userId).subscribe({
         next: (response: Recipe[]) => {
           this.recipes = response.map(recipe => ({
             name : recipe.name,
             id: recipe.id,
             createdAt: recipe.createdAt,
             userId: recipe.userId,
             description: recipe.description,
             favourite: recipe.favourite,
             category: recipe.category,
             ingredients: recipe.ingredients || [], 
             imageUrl: '/assets/images/' + recipe.imageUrl // Dodajte prefiks na imageUrl za svaki recept
           }));
           this.filteredRecipes = this.recipes;
         },
         error: (error) => {      
            this.processError(error);
         }
       });  
     }

     ngOnInit(): void {
      this.userservice.loadCurrentUser().subscribe({
        next: (response) => {
            this.userId = response.id;
            this.loadRecipes();
        },
        error: (error) => {
          this.processError(error);
        }
      });
     }
   
     onSearch() {
        this.filteredRecipes = this.recipes.filter(recipe =>
          recipe.name.toLowerCase().includes(this.searchTerm.toLowerCase()));
          if(this.selectedCategory && this.selectedCategory != 'All')
          {
           this.filteredRecipes =  this.filteredRecipes.filter(recipe =>
             recipe.category == this.selectedCategory); 
          }
      }
    
      showRecipeDetails(recipeId: string) {
       this.router.navigate(['/auth/recipe-details', recipeId]);
     }  
     toggleFavorite(recipeId: string, event: Event) {
      event.stopPropagation();
      const foundRecipe = this.recipes.find((recipe) => recipe.id === recipeId);
  
      if (foundRecipe &&  foundRecipe.favourite) {
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
    console.error(message);
  }
}

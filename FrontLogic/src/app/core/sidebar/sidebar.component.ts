import { Component, OnInit, Input } from '@angular/core';
import { RecipeService } from '../../services/recipeservice';
import { Recipe } from '../../models/recipe';
import { Router } from '@angular/router';


@Component({
  selector: 'cdk-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent  implements OnInit {
    latestRecipes: any[] = [];
    topRatedRecipes: any[] = [];
  
    constructor(private recipeService: RecipeService, private router: Router) {  }
  
    ngOnInit() {
      this.recipeService.getLatestRecipes(2).subscribe((recipes: Recipe[]) => {
        this.latestRecipes = recipes.map(recipe => ({
          name: recipe.name,
          createdAt: recipe.createdAt,
          id: recipe.id,
          userId: recipe.userId,
          description: recipe.description,
          favourite: recipe.favourite,
          category: recipe.category,
          imageUrl: '/assets/images/' + recipe.imageUrl 
        }));
      });
    
      this.recipeService.getTopRatedRecipes(2).subscribe((recipes: Recipe[]) => {
        this.topRatedRecipes = recipes.map(recipe => ({
          name: recipe.name,
          createdAt: recipe.createdAt,
          id: recipe.id,
          userId: recipe.userId,
          description: recipe.description,
          favourite: recipe.favourite,
          category: recipe.category,
          imageUrl: '/assets/images/' + recipe.imageUrl 
        }));
      });
    }

    navigateToRecipeDetails(recipeId: string)
    {
      this.router.navigate(['/auth/recipe-details', recipeId]);
    }
  }
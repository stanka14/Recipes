import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Recipe, RecipeCategoryEnum } from '../models/recipe';
import { RecipeService } from '../services/recipeservice';
import { RatingsAndCommentsService } from '../services/ratingsandcommentsservice';
import { UserService } from '../services/userservice';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-recipe-details',
  templateUrl: './recipe-details.component.html',
  styleUrls: ['./recipe-details.component.scss']
})
export class RecipeDetailsComponent implements OnInit {
  recipeId: string = '';
  imageUrl: string = '';
  recipeForm: FormGroup;
  recipeRating: number = 0;
  newComment: string = '';
  errorMessage: string | null = null;
  rateErrorMessage: string | null = null;
  recipeCategories = Object.values(RecipeCategoryEnum).filter(item => typeof item === 'string');
  recipeUserId: string | null = null;
  currentUserId: string | null = null;
  favourite: boolean = false;
  recipeComments: any[] = [];
  pageSize = 3;
  pageSizeOptions: number[] = [3, 5, 10, 25];
  currentPage = 1;
  pagedComments: any[] = [];

  constructor(
    private userservice: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private recipeservice: RecipeService,
    private ratingsAndCommentsService: RatingsAndCommentsService,
    private snackBar: MatSnackBar
  ) {
    this.recipeForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: ['', Validators.required],
      ingredients: this.fb.array([])  
    });
  }

  private handleRouteChange() {
    this.loadData();
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.recipeId = params.get('id')!;
      this.loadData();
    });
    this.route.queryParams.subscribe(params => {
      if (params.refresh) {
        this.handleRouteChange();
      } else {
        this.loadData();
      }
    });
  }

  addIngredient() {
    const ingredients = this.recipeForm.get('ingredients') as FormArray;
    ingredients.push(this.fb.group({
      name: ['', Validators.required],
      quantity: ['', Validators.required],
      id: ['']
    }));
  }

  removeIngredient(index: number) {
    const ingredients = this.recipeForm.get('ingredients') as FormArray;
    ingredients.removeAt(index);
  }

  loadData()
  {
    this.userservice.loadCurrentUser().subscribe({
      next: (response) => {
        this.currentUserId = response.id;

        this.recipeservice.getSelectedRecipe(this.recipeId).subscribe({
          next: (response: Recipe) => {
            this.recipeForm.patchValue({
              name: response.name,
              category: response.category.toString(),
              description: response.description,
            });
            const ingredientsArray = response.ingredients.map(ingredient => {
              return this.fb.group({
                name: ingredient.name,
                quantity: ingredient.quantity,
                id: ingredient.id
              });
            });
            this.recipeForm.setControl('ingredients', this.fb.array(ingredientsArray));
            this.imageUrl = '/assets/images/' + response.imageUrl!;
            this.recipeUserId = response.userId;
            this.favourite = response.favourite;
            this.recipeForm.enable();

            if (this.currentUserId !== this.recipeUserId) {
              this.recipeForm.disable();
            }

            this.loadRatings();
            this.loadComments();
            this.errorMessage = '';
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

  onPageChange(event: any) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex + 1;
    this.updatePagedComments();
  }

  updatePagedComments(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedComments = this.recipeComments.slice(startIndex, endIndex);
  }

  saveChanges() {
    if (this.currentUserId !== this.recipeUserId) {
      this.alertError('You are not the owner of this recipe.');
      return;
    }
  
    if (this.recipeForm.valid) {
      const recipeDto = {
        description: this.recipeForm.value.description,
        name: this.recipeForm.value.name,
        category: this.recipeForm.value.category,
        imageUrl: this.imageUrl,
        userId: this.currentUserId,
        id: this.recipeId,
        favourite: this.favourite,
        ingredients: this.recipeForm.value.ingredients || [],  
      };

      this.recipeservice.updateSelectedRecipe(recipeDto).subscribe({
        next: (response) => {
          this.router.navigate(['/auth/recipe-list'], { queryParams: { 'updated': 'true' } });
        },
        error: (error) => {
         this.processError(error);    
        }
      });
    } else {
      this.recipeForm.markAllAsTouched();
    }
  }

  deleteRecipe() {
    if (this.currentUserId !== this.recipeUserId) {
      this.alertError('You are not the owner of this recipe.');
      return;
    }
    this.recipeservice.deleteSelectedRecipe(this.recipeId).subscribe({
      next: (response) => {
        this.router.navigate(['/auth/recipe-list'], { queryParams: { 'updated': 'true' } });
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.recipeForm.patchValue({
        imageUrl: file
      });

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.recipeForm.patchValue({
          imageUrl: e.target.result
        });
      };
      reader.readAsDataURL(file);
    }
  }

  loadRatings(): void {
    this.ratingsAndCommentsService.getallRecipeRatings(this.currentUserId, this.recipeId).subscribe(ratings => {
      this.recipeRating = this.calculateAverageRating(ratings);
    });
  }

  loadComments(): void {
    this.ratingsAndCommentsService.getallRecipeComments(this.currentUserId, this.recipeId).subscribe(comments => {
      this.recipeComments = comments;
      this.updatePagedComments();
    });
  }

  rateRecipe(rating: number): void {
    if (this.currentUserId == this.recipeUserId) {
      this.rateErrorMessage = 'You cannot rate your own recipe.';
      return;
    }
    this.recipeRating = rating;
    this.addRating();
  }

  addRating(): void {
    const ratingDto = {
      id: '',
      rating: this.recipeRating,
      recipeId: this.recipeId,
      userId: this.currentUserId
    };

    this.ratingsAndCommentsService.addRating(ratingDto).subscribe({
      next: (response) => {
        this.loadRatings();
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  addComment(): void {
    const commentDto = {
      id: '',
      content: this.newComment,
      recipeId: this.recipeId,
      userId: this.currentUserId,
      userName: ''
    };

    this.ratingsAndCommentsService.addComment(commentDto).subscribe({
      next: (response) => {
        this.loadComments();
        this.newComment = '';
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  calculateAverageRating(ratings: any[]): number {
    if (ratings.length === 0) {
      return 0;
    }
    const sumOfRatings = ratings.reduce((total, rating) => total + rating.rating, 0);
    const averageRating = sumOfRatings / ratings.length;
    return parseFloat(averageRating.toFixed(2));
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

import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './auth.component';
import { DashboardCrmComponent } from '../dashboard-crm/dashboard-crm.component';
import { GuardedRoutesComponent } from '../guarded-routes/guarded-routes.component';
import { RecipeListComponent } from '../recipes/recipe-list.component';
import { AddRecipeComponent } from '../add-recipe/add-recipe.component';
import { RecipeDetailsComponent } from '../recipe-details/recipe-details.component';
import { UserProfileComponent } from '../user-profile/user-profile.component';
import { ChatComponent } from '../chat/chat.component';
import { AllRecipesComponent } from '../allrecipes/allrecipes.component';
import { ShoppingListComponent } from '../shopping-list/shopping-list.component';

export const appRoutes: Routes = [{
    path: '', component: AuthComponent, children: [
        { path: 'dashboard', component: DashboardCrmComponent, canActivate: [GuardedRoutesComponent] },
        { path: 'recipe-list', component: RecipeListComponent ,data: { animation: 'recipe-list' }, canActivate: [GuardedRoutesComponent]},
        { path: 'allrecipes', component: AllRecipesComponent ,data: { animation: 'allrecipes' }, canActivate: [GuardedRoutesComponent]},
        { path: 'chat', component: ChatComponent ,data: { animation: 'chat' }, canActivate: [GuardedRoutesComponent]},
        { path: 'shopping-list', component: ShoppingListComponent ,data: { animation: 'chat' }, canActivate: [GuardedRoutesComponent]},
        { path: 'recipe-add', component: AddRecipeComponent ,data: { animation: 'recipe-add' }, canActivate: [GuardedRoutesComponent]},
        { path: 'recipe-details/:id', component: RecipeDetailsComponent, data: { animation: 'recipe-details' }, canActivate: [GuardedRoutesComponent] },
        { path: 'user-details', component: UserProfileComponent, canActivate: [GuardedRoutesComponent] },
        { path: 'material-widgets', loadChildren: () => import('../material-widgets/material-widgets.module').then(m => m.MaterialWidgetsModule) },
        { path: 'guarded-routes', loadChildren: () => import('../guarded-routes/guarded-routes.module').then(m => m.GuardedRoutesModule) },
    ]
}];

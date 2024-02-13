import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { Routes, RouterModule } from '@angular/router';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatChipsModule } from '@angular/material/chips';
import { CoreModule } from '../core/core.module';
import { AddRecipeComponent } from './add-recipe.component';
import { FormsModule , ReactiveFormsModule} from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';

const routes: Routes = [
    {path: '', component: AddRecipeComponent},
  ];
@NgModule({
    imports: [
        MatCheckboxModule,
        CoreModule,
        MatSnackBarModule,
        MatIconModule,
        MatChipsModule,
        MatToolbarModule,
        MatCardModule,
        CommonModule,
        FlexLayoutModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatInputModule,
        MatListModule,
        MatDividerModule,
        FormsModule,
        ReactiveFormsModule,
        MatOptionModule,
        MatSelectModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        AddRecipeComponent
    ],
    exports: [
        RouterModule
    ],
    providers: [
        
    ]
})
export class AddRecipeModule {
}

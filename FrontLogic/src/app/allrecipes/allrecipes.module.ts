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
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AllRecipesComponent } from './allrecipes.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';

const routes: Routes = [
    {path: '', component: AllRecipesComponent},
  ];
@NgModule({
    imports: [
        MatCheckboxModule,
        CoreModule,
        MatIconModule,
        MatChipsModule,
        MatToolbarModule,
        MatCardModule,
        MatSnackBarModule,
        FormsModule,
        ReactiveFormsModule,
        MatSelectModule,
        MatOptionModule,
        CommonModule,
        FlexLayoutModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatInputModule,
        MatListModule,
        MatDividerModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        AllRecipesComponent
    ],
    exports: [
        RouterModule
    ],
    providers: [
        
    ]
})
export class AllRecipesModule {
}

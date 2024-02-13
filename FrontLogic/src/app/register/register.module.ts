import { NgModule } from '@angular/core';
import { RegisterComponent } from './register.component';
import { RouterModule, Routes } from '@angular/router'; 
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatError } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule , ReactiveFormsModule} from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';

const appRoutes: Routes = [
    { path: '', component: RegisterComponent },
]

@NgModule({
  imports: [
    CommonModule,
    MatCardModule,
    CommonModule,
    FlexLayoutModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatInputModule,
    MatToolbarModule,
    FormsModule,
    MatSnackBarModule,
    ReactiveFormsModule,
    RouterModule.forChild(appRoutes),
  ],
  declarations: [RegisterComponent]
})
export class RegisterModule { }

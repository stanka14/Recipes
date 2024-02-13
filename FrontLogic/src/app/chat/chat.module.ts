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
import { FormsModule , ReactiveFormsModule} from '@angular/forms';
import { ChatComponent } from './chat.component';
import { MatPaginatorModule } from '@angular/material/paginator';
const routes: Routes = [
    {path: '', component: ChatComponent},
  ];
@NgModule({
    imports: [
        MatCheckboxModule,
        CoreModule,
        MatIconModule,
        MatPaginatorModule,
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
        RouterModule.forChild(routes)
    ],
    declarations: [
        ChatComponent
    ],
    exports: [
        RouterModule
    ],
    providers: [
        
    ]
})
export class ChatModule {
}

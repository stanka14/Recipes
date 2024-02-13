import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidemenuComponent } from './sidemenu/sidemenu.component';
import { SidemenuItemComponent } from './sidemenu-item/sidemenu-item.component';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTabsModule } from '@angular/material/tabs';
import { RouterModule } from '@angular/router';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatCardModule } from '@angular/material/card';
import { NotificationsComponent } from './toolbar-notification/toolbar-notification.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FullscreenComponent } from './fullscreen/fullscreen.component';
import { UserMenuComponent } from './user-menu/user-menu.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSliderModule } from '@angular/material/slider';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({

    declarations: [
        SidemenuComponent,
        SidemenuItemComponent,
        NotificationsComponent,
        ToolbarComponent,
        SearchBarComponent,
        SidebarComponent,
        FullscreenComponent,
        UserMenuComponent
    ],

    imports: [
        CommonModule,
        MatListModule,
        MatButtonModule,
        MatSnackBarModule,
        MatInputModule,
        MatIconModule,
        MatChipsModule,
        RouterModule,
        NgScrollbarModule,
        FlexLayoutModule,
        MatCardModule,
        MatToolbarModule,
        MatFormFieldModule,
        MatSidenavModule,
        MatTabsModule,
        MatSliderModule,
        MatProgressBarModule,
        MatListModule,
        MatDividerModule
    ],


    exports: [
        SidemenuComponent,
        SidemenuItemComponent,
        NotificationsComponent,
        ToolbarComponent,
        SearchBarComponent,
        SidebarComponent,
        FullscreenComponent,
        UserMenuComponent,
    ],

    providers: [
        // {
        //     provide: PERFECT_SCROLLBAR_CONFIG,
        //     useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
        // }
    ]
})
export class CoreModule { }

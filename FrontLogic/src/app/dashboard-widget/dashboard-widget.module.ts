import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DashcardComponent } from './dashcard/dashcard.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatChipsModule} from '@angular/material/chips';
import { MatProgressBarModule} from '@angular/material/progress-bar';
import { ProfileCardComponent } from './profile-card/profile-card.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { WeatherComponent } from './weather/weather.component';
import { RoundProgressbarComponent } from './round-progressbar/round-progressbar.component'; 
import { PricingPlanComponent } from './pricing-plan/pricing-plan.component';
import { RoundProgressModule } from 'angular-svg-round-progressbar';
import { D3UsaComponent } from './d3-usa/d3-usa.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';


@NgModule({
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatTabsModule,
    MatToolbarModule,
    MatListModule,
    MatMenuModule,
    MatChipsModule,
    MatProgressBarModule,
    NgxChartsModule,
    RoundProgressModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    DashcardComponent, 
    ProfileCardComponent,
    PricingPlanComponent,
    WeatherComponent,
    RoundProgressbarComponent,
    D3UsaComponent
  ],
  exports: [
    DashcardComponent, 
    ProfileCardComponent,
    PricingPlanComponent,
    WeatherComponent,
    RoundProgressbarComponent,
    D3UsaComponent
  ]
})
export class DashboardWidgetModule { }

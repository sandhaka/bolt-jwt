import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts/ng2-charts';

import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard.routing';
import {HttpClientModule} from "@angular/common/http";

@NgModule({
  imports: [
    DashboardRoutingModule,
    ChartsModule,
    HttpClientModule
  ],
  declarations: [ DashboardComponent ]
})
export class DashboardModule { }

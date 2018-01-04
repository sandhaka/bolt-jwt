import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard.routing';
import {TokenLogsService} from "./token-logs.service";
import {SharedModule} from "../../shared/shared.module";
import {ModalModule} from "ngx-bootstrap";
import {ErrorModalComponent} from "../../shared/modals/error/error-modal.component";
import {CommonModule} from "@angular/common";

@NgModule({
  imports: [
    CommonModule,
    DashboardRoutingModule,
    ChartsModule,
    ModalModule.forRoot(),
    SharedModule
  ],
  declarations: [ DashboardComponent ],
  entryComponents: [
    ErrorModalComponent
  ],
  providers: [
    TokenLogsService
  ]
})
export class DashboardModule { }

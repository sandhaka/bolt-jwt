
import {NgModule} from "@angular/core";
import {ModalComponent} from "./modals/modal.component";
import {ModalModule} from "ngx-bootstrap/modal";
import {CommonModule} from "@angular/common";
import { DataTableComponent } from './datatable/data-table.component';
import {NgxDatatableModule} from "@swimlane/ngx-datatable";

@NgModule({
  imports: [
    ModalModule,
    CommonModule,
    NgxDatatableModule
  ],
  declarations: [
    ModalComponent,
    DataTableComponent
  ],
  exports: [
    ModalComponent,
    DataTableComponent
  ]
})
export class SharedModule { }

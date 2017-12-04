
import {NgModule} from "@angular/core";
import {ModalComponent} from "./modals/modal.component";
import {ModalModule} from "ngx-bootstrap/modal";
import {CommonModule} from "@angular/common";
import { DataTableComponent } from './datatable/data-table.component';
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {ReactiveFormComponent} from "./base/reactiveForm.component";

@NgModule({
  imports: [
    ModalModule,
    CommonModule,
    NgxDatatableModule
  ],
  declarations: [
    ModalComponent,
    DataTableComponent,
    ReactiveFormComponent
  ],
  exports: [
    ModalComponent,
    DataTableComponent,
    ReactiveFormComponent
  ]
})
export class SharedModule { }

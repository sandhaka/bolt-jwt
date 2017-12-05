
import {NgModule} from "@angular/core";
import {ModalComponent} from "./modals/modal.component";
import {ModalModule} from "ngx-bootstrap/modal";
import {CommonModule} from "@angular/common";
import { DataTableComponent } from './datatable/data-table.component';
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {ReactiveFormComponent} from "./base/reactiveForm.component";
import {DataTableFiltersComponent} from "./datatable/filters/data-table-filters.component";
import {ReactiveFormsModule} from "@angular/forms";
import {DataTableFiltersStringComponent} from "./datatable/filters/fields";

@NgModule({
  imports: [
    ModalModule,
    CommonModule,
    ReactiveFormsModule,
    NgxDatatableModule
  ],
  declarations: [
    ModalComponent,
    DataTableComponent,
    DataTableFiltersComponent,
    DataTableFiltersStringComponent,
    ReactiveFormComponent
  ],
  exports: [
    ModalComponent,
    DataTableComponent,
    DataTableFiltersComponent,
    ReactiveFormComponent
  ]
})
export class SharedModule { }

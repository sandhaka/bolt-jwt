
import {NgModule} from "@angular/core";
import {ModalModule} from "ngx-bootstrap/modal";
import {CommonModule} from "@angular/common";
import { DataTableComponent } from './datatable/data-table.component';
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {ReactiveFormComponent} from "./base/reactiveForm.component";
import {DataTableFiltersComponent} from "./datatable/filters/data-table-filters.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {DataTableFiltersStringComponent} from "./datatable/filters/fields";
import {GenericModalComponent, ConfirmModalComponent} from "./modals";
import {AuthorizationsManagerComponent} from "./components/authorizations-manager/authorizations-manager.component";
import {UtilityService} from "./utils.service";
import {LoadingModule} from "ngx-loading";

@NgModule({
  imports: [
    ModalModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxDatatableModule,
    LoadingModule
  ],
  declarations: [
    GenericModalComponent,
    ConfirmModalComponent,
    DataTableComponent,
    DataTableFiltersComponent,
    DataTableFiltersStringComponent,
    ReactiveFormComponent,
    AuthorizationsManagerComponent
  ],
  exports: [
    GenericModalComponent,
    ConfirmModalComponent,
    DataTableComponent,
    DataTableFiltersComponent,
    ReactiveFormComponent,
    AuthorizationsManagerComponent
  ],
  providers: [
    UtilityService
  ]
})
export class SharedModule { }

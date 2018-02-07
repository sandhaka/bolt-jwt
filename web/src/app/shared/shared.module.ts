
import {NgModule} from "@angular/core";
import {ModalModule} from "ngx-bootstrap/modal";
import {CommonModule} from "@angular/common";
import { DataTableComponent } from './datatable/data-table.component';
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {ReactiveFormComponent} from "./base/reactiveForm.component";
import {DataTableFiltersComponent} from "./datatable/filters/data-table-filters.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {DataTableFiltersStringComponent} from "./datatable/filters/fields";
import {GenericModalComponent, ConfirmModalComponent, ErrorModalComponent} from "./modals";
import {AuthorizationsManagerComponent} from "./components/authorizations-manager/authorizations-manager.component";
import {UtilityService} from "./utils.service";
import {LoadingModule} from "ngx-loading";
import {RolesManagerModalComponent} from "./components/roles-manager-modal/roles-manager-modal.component";

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
    ErrorModalComponent,
    DataTableComponent,
    DataTableFiltersComponent,
    DataTableFiltersStringComponent,
    ReactiveFormComponent,
    AuthorizationsManagerComponent,
    RolesManagerModalComponent
  ],
  exports: [
    GenericModalComponent,
    ConfirmModalComponent,
    ErrorModalComponent,
    DataTableComponent,
    DataTableFiltersComponent,
    ReactiveFormComponent,
    AuthorizationsManagerComponent,
    RolesManagerModalComponent
  ],
  providers: [
    UtilityService
  ]
})
export class SharedModule { }

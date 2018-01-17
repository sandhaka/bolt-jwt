import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ModalModule} from "ngx-bootstrap";
import {SharedModule} from "../../shared/shared.module";
import {LaddaModule} from "angular2-ladda";
import {LoadingModule} from "ngx-loading";
import {ErrorModalComponent} from "../../shared/modals/error/error-modal.component";
import {ConfirmModalComponent, GenericModalComponent} from "../../shared/modals";
import {RolesRoutingModule} from "./roles.routing";
import {ReactiveFormsModule} from "@angular/forms";
import {RolesComponent} from "./roles.component";
import {RolesService} from "./roles.service";
import {RoleDetailsComponent} from "./role-details/role-details.component";
import {CreateRoleModalComponent} from "./create-role-modal/create-role-modal.component";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    SharedModule,
    RolesRoutingModule,
    LoadingModule,
    LaddaModule
  ],
  declarations: [
    RolesComponent,
    RoleDetailsComponent,
    CreateRoleModalComponent
  ],
  entryComponents: [
    ErrorModalComponent,
    GenericModalComponent,
    ConfirmModalComponent,
    CreateRoleModalComponent
  ],
  providers: [
    RolesService
  ]
})
export class RolesModule {  }

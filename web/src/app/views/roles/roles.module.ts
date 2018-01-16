import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ModalModule} from "ngx-bootstrap";
import {SharedModule} from "../../shared/shared.module";
import {LaddaModule} from "angular2-ladda";
import {LoadingModule} from "ngx-loading";
import {ErrorModalComponent} from "../../shared/modals/error/error-modal.component";
import {GenericModalComponent} from "../../shared/modals";
import {RolesRoutingModule} from "./roles.routing";
import {ReactiveFormsModule} from "@angular/forms";
import {RolesComponent} from "./roles.component";
import {RolesService} from "./roles.service";

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
    RolesComponent
  ],
  entryComponents: [
    ErrorModalComponent,
    GenericModalComponent
  ],
  providers: [
    RolesService
  ]
})
export class RolesModule {  }

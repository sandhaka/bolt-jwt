import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ModalModule} from "ngx-bootstrap";
import {AuthorizationsRoutingModule} from "./authorizations.routing";
import {SharedModule} from "../../shared/shared.module";
import {AuthorizationsComponent} from "./authorizations.component";
import {ConfirmModalComponent, GenericModalComponent} from "../../shared/modals";
import {AuthorizationsService} from "./authorizations.service";
import { CreateAuthModalComponent } from './create-auth-modal/create-auth-modal.component';
import {FormsModule} from "@angular/forms";
import { ViewAuthUsageModalComponent } from './view-auth-usage-modal/view-auth-usage-modal.component';
import {ErrorModalComponent} from "../../shared/modals/error/error-modal.component";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ModalModule.forRoot(),
    AuthorizationsRoutingModule,
    SharedModule
  ],
  declarations: [
    AuthorizationsComponent,
    CreateAuthModalComponent,
    ViewAuthUsageModalComponent
  ],
  entryComponents: [
    GenericModalComponent,
    ErrorModalComponent,
    CreateAuthModalComponent,
    ViewAuthUsageModalComponent,
    ConfirmModalComponent
  ],
  providers: [
    AuthorizationsService
  ]
})
export class AuthorizationsModule { }

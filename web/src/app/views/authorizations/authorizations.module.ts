import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ModalModule} from "ngx-bootstrap";
import {AuthorizationsRoutingModule} from "./authorizations.routing";
import {SharedModule} from "../../shared/shared.module";
import {AuthorizationsComponent} from "./authorizations.component";
import {GenericModalComponent} from "../../shared/modals";
import {AuthorizationsService} from "./authorizations.service";

@NgModule({
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    AuthorizationsRoutingModule,
    SharedModule
  ],
  declarations: [
    AuthorizationsComponent
  ],
  entryComponents: [
    GenericModalComponent
  ],
  providers: [
    AuthorizationsService
  ]
})
export class AuthorizationsModule { }

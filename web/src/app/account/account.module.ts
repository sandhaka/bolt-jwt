import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ReactiveFormsModule} from "@angular/forms";
import {ModalModule} from "ngx-bootstrap";
import {SharedModule} from "../shared/shared.module";
import {AccountActivationComponent} from "./account-activation/account-activation.component";
import {AccountRoutingModule} from "./account.routing";
import {AccountService} from "./account.service";
import {GenericModalComponent} from "../shared/modals";
import {LaddaModule} from "angular2-ladda";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    AccountRoutingModule,
    SharedModule,
    LaddaModule
  ],
  declarations: [
    AccountActivationComponent
  ],
  entryComponents: [
    GenericModalComponent
  ],
  providers: [
    AccountService
  ]
})
export class AccountModule { }

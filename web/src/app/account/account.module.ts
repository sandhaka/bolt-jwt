import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ReactiveFormsModule} from "@angular/forms";
import {ModalModule} from "ngx-bootstrap";
import {SharedModule} from "../shared/shared.module";
import {AccountActivationComponent} from "./account-activation/account-activation.component";
import {AccountRoutingModule} from "./account.routing";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    AccountRoutingModule,
    SharedModule
  ],
  declarations: [
    AccountActivationComponent
  ],
  entryComponents: [

  ]
})
export class AccountModule { }

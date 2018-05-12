import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ReactiveFormsModule} from "@angular/forms";
import {ModalModule} from "ngx-bootstrap";
import {SharedModule} from "../shared/shared.module";
import {AccountActivationComponent} from "./activation/account-activation.component";
import {AccountRoutingModule} from "./account.routing";
import {AccountService} from "./account.service";
import {ErrorModalComponent, GenericModalComponent} from "../shared/modals";
import {LaddaModule} from "angular2-ladda";
import {AccountActivatedConfirmationComponent} from "./confirmed/account-activated-confirmation.component";
import {ForgotPasswordComponent} from "./forgot-password/forgot-password.component";
import {PasswordRecoveryComponent} from "./password-recovery/password-recovery.component";

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
    AccountActivationComponent,
    AccountActivatedConfirmationComponent,
    ForgotPasswordComponent,
    PasswordRecoveryComponent
  ],
  entryComponents: [
    GenericModalComponent,
    ErrorModalComponent
  ],
  providers: [
    AccountService
  ]
})
export class AccountModule { }

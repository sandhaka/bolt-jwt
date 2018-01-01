import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AccountActivationComponent} from "./activation/account-activation.component";
import {AccountActivatedConfirmationComponent} from "./confirmed/account-activated-confirmation.component";
import {ForgotPasswordComponent} from "./forgot-password/forgot-password.component";
import {PasswordRecoveryComponent} from "./password-recovery/password-recovery.component";

const routes: Routes = [
  {
    path: 'activation/:code',
    component: AccountActivationComponent,
    data: {
      title: 'Activation'
    }
  },
  {
    path: 'confirmed',
    component: AccountActivatedConfirmationComponent,
    data: {
      title: 'Confirmed'
    }
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
    data: {
      title: 'Password recovery'
    }
  },
  {
    path: 'password-recovery/:userId/:code',
    component: PasswordRecoveryComponent,
    data: {
      title: 'Password recovery'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule {}

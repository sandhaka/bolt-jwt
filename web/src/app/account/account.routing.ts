import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AccountActivationComponent} from "./activation/account-activation.component";
import {AccountActivatedConfirmationComponent} from "./confirmed/account-activated-confirmation.component";

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
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule {}

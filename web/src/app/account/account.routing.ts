import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AccountActivationComponent} from "./account-activation/account-activation.component";

const routes: Routes = [
  {
    path: 'activation/:code',
    component: AccountActivationComponent,
    data: {
      title: 'activation'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule {}

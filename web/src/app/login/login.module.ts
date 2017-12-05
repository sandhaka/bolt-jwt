import {NgModule} from '@angular/core';

import {LoginComponent} from './login.component';
import {LoginRoutingModule} from './login.routing';
import {ReactiveFormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {SharedModule} from "../shared/shared.module";
import {ModalModule} from "ngx-bootstrap/modal";
import {GenericModalComponent} from "../shared/modals";

@NgModule({
  imports: [
    CommonModule,
    LoginRoutingModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    SharedModule
  ],
  declarations: [LoginComponent],
  entryComponents: [GenericModalComponent]
})
export class LoginModule { }

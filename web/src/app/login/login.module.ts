import {NgModule} from '@angular/core';

import {LoginComponent} from './login.component';
import {LoginRoutingModule} from './login.routing';
import {ReactiveFormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {HttpClientModule} from '@angular/common/http';
import {SharedModule} from "../shared/shared.module";
import {ModalModule} from "ngx-bootstrap/modal";
import {ModalComponent} from "../shared/modals/modal.component";

@NgModule({
  imports: [
    CommonModule,
    LoginRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    SharedModule
  ],
  declarations: [LoginComponent],
  entryComponents: [ModalComponent]
})
export class LoginModule { }

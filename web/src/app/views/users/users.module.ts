import { NgModule } from '@angular/core';
import {ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {ModalModule} from "ngx-bootstrap";
import {ConfirmModalComponent, GenericModalComponent} from "../../shared/modals";
import {SharedModule} from "../../shared/shared.module";
import {UsersRoutingModule} from "./users.routing";
import {UsersService} from "./users.service";
import {UsersComponent} from "./users.component";
import {UserDetailsComponent} from "./user-details/user-details.component";
import { CreateUserModalComponent } from './create-user-modal/create-user-modal.component';
import {LoadingModule} from "ngx-loading";
import {LaddaModule} from "angular2-ladda";
import {ErrorModalComponent} from "../../shared/modals/error/error-modal.component";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    UsersRoutingModule,
    SharedModule,
    LoadingModule,
    LaddaModule
  ],
  declarations: [
    UsersComponent,
    UserDetailsComponent,
    CreateUserModalComponent
  ],
  entryComponents: [
    GenericModalComponent,
    ErrorModalComponent,
    ConfirmModalComponent,
    CreateUserModalComponent
  ],
  providers: [
    UsersService
  ]
})
export class UsersModule { }

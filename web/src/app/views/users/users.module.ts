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

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    UsersRoutingModule,
    SharedModule
  ],
  declarations: [
    UsersComponent,
    UserDetailsComponent
  ],
  entryComponents: [
    GenericModalComponent,
    ConfirmModalComponent
  ],
  providers: [
    UsersService
  ]
})
export class UsersModule { }

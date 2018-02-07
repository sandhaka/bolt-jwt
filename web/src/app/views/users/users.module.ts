import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {ModalModule} from "ngx-bootstrap";
import {ConfirmModalComponent, GenericModalComponent, ErrorModalComponent} from "../../shared/modals";
import {SharedModule} from "../../shared/shared.module";
import {UsersRoutingModule} from "./users.routing";
import {UsersService} from "./users.service";
import {UsersComponent} from "./users.component";
import {UserDetailsComponent} from "./user-details/user-details.component";
import { CreateUserModalComponent } from './create-user-modal/create-user-modal.component';
import {LoadingModule} from "ngx-loading";
import {LaddaModule} from "angular2-ladda";
import {RolesManagerModalComponent} from "../../shared/components/roles-manager-modal/roles-manager-modal.component";
import {GroupsManagerModalComponent} from "./groups-manager-modal/groups-manager-modal.component";
import {GroupsManagerModalService} from "./groups-manager-modal/groups-manager-modal.service";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
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
    CreateUserModalComponent,
    GroupsManagerModalComponent
  ],
  entryComponents: [
    GenericModalComponent,
    ErrorModalComponent,
    ConfirmModalComponent,
    CreateUserModalComponent,
    RolesManagerModalComponent,
    GroupsManagerModalComponent
  ],
  providers: [
    UsersService,
    GroupsManagerModalService
  ]
})
export class UsersModule { }

import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ReactiveFormsModule} from "@angular/forms";
import {ModalModule} from "ngx-bootstrap";
import {SharedModule} from "../../shared/shared.module";
import {LoadingModule} from "ngx-loading";
import {LaddaModule} from "angular2-ladda";
import {ErrorModalComponent} from "../../shared/modals/error/error-modal.component";
import {ConfirmModalComponent, GenericModalComponent} from "../../shared/modals";
import {GroupDetailsComponent} from "./group-details/group-details.component";
import {GroupsService} from "./groups.service";
import {GroupsComponent} from "./groups.component";
import {GroupsRoutingModule} from "./groups.routing";
import {RolesManagerModalComponent} from "../../shared/components/roles-manager-modal/roles-manager-modal.component";
import {CreateGroupModalComponent} from "./create-group-modal/create-group-modal.component";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    SharedModule,
    GroupsRoutingModule,
    LoadingModule,
    LaddaModule
  ],
  declarations: [
    GroupDetailsComponent,
    GroupsComponent,
    CreateGroupModalComponent
  ],
  entryComponents: [
    ErrorModalComponent,
    GenericModalComponent,
    ConfirmModalComponent,
    RolesManagerModalComponent,
    CreateGroupModalComponent
  ],
  providers: [
    GroupsService
  ]
})
export class GroupsModule { }

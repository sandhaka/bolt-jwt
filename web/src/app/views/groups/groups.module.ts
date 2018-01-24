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
    GroupsComponent
  ],
  entryComponents: [
    ErrorModalComponent,
    GenericModalComponent,
    ConfirmModalComponent
  ],
  providers: [
    GroupsService
  ]
})
export class GroupsModule { }

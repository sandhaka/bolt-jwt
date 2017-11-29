import { NgModule } from '@angular/core';
import {HttpClientModule} from "@angular/common/http";
import {ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {ModalModule} from "ngx-bootstrap";
import {ModalComponent} from "../../shared/modals/modal.component";
import {SharedModule} from "../../shared/shared.module";
import {UsersRoutingModule} from "./users.routing";
import {UsersService} from "./users.service";
import {NgxDatatableModule} from "@swimlane/ngx-datatable";

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    UsersRoutingModule,
    NgxDatatableModule,
    SharedModule
  ],
  declarations: [],
  entryComponents: [
    ModalComponent
  ],
  providers: [
    UsersService
  ]
})
export class UsersModule { }

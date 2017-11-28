
import {NgModule} from "@angular/core";
import {ModalComponent} from "./modals/modal.component";
import {ModalModule} from "ngx-bootstrap/modal";
import {CommonModule} from "@angular/common";
import {UtilityService} from "./utils.service";

@NgModule({
  imports: [
    ModalModule,
    CommonModule
  ],
  declarations: [
    ModalComponent
  ],
  exports: [
    ModalComponent
  ]
})
export class SharedModule { }

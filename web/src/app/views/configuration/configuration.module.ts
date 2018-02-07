import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ReactiveFormsModule} from "@angular/forms";
import {ModalModule} from "ngx-bootstrap";
import {SharedModule} from "../../shared/shared.module";
import {LoadingModule} from "ngx-loading";
import {ConfigurationComponent} from "./configuration.component";
import {GenericModalComponent, ErrorModalComponent} from "../../shared/modals";
import {ConfigurationRoutingModule} from "./configuration.routing";
import {ConfigurationService} from "./configuration.service";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    ConfigurationRoutingModule,
    SharedModule,
    LoadingModule
  ],
  declarations: [
    ConfigurationComponent
  ],
  entryComponents: [
    GenericModalComponent,
    ErrorModalComponent
  ],
  providers: [
    ConfigurationService
  ]
})
export class ConfigurationModule { }

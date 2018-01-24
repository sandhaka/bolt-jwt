import {NgModule} from "@angular/core";
import {ForbiddenComponent} from "./forbidden.component";
import {ErrorPagesRouting} from "./error-pages.routing";

@NgModule({
  imports: [
    ErrorPagesRouting
  ],
  declarations: [
    ForbiddenComponent
  ]
})
export class ErrorPagesModule { }

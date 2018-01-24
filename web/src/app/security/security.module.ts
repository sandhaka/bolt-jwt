import {NgModule} from '@angular/core';
import {AuthGuardService} from './auth.guard.service';
import {SecurityService} from './security.service';
import {AuthRootGuardService} from "./auth.root-guard.service";

@NgModule({
  imports: [],
  providers: [
    AuthGuardService,
    AuthRootGuardService,
    SecurityService
  ]
})
export class SecurityModule { }

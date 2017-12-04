import {NgModule} from '@angular/core';
import {AuthGuardService} from './auth.guard.service';
import {SecurityService} from './security.service';

@NgModule({
  imports: [],
  providers: [
    AuthGuardService,
    SecurityService
  ]
})
export class SecurityModule { }

import {NgModule} from '@angular/core';
import {AuthGuardService} from './auth.guard.service';
import {AuthenticationService} from './authentication.service';
import {HttpClientModule} from '@angular/common/http';

@NgModule({
  imports: [
    HttpClientModule
  ],
  providers: [
    AuthGuardService,
    AuthenticationService
  ]
})
export class SecurityModule { }

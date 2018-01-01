import { Component } from '@angular/core';
import {SecurityService} from "../../security/security.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html'
})
export class AppHeaderComponent {

  constructor(
    private securityService: SecurityService,
    private router: Router) { }

  logout() {
    this.securityService.logout();
    this.router.navigate(['/login']);
  }
}

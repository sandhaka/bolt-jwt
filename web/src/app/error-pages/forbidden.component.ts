import {Component} from "@angular/core";
import {SecurityService} from "../security/security.service";
import {Router} from "@angular/router";

@Component({
  template: `
    <div class="app flex-row align-items-center">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-md-6">
            <div class="clearfix">
              <h1 class="float-left display-3 mr-4">403</h1>
              <h4 class="pt-3">Oops! You're unauthorized.</h4>
              <p class="text-muted">This section is reserved for system administrators</p>
            </div>
            <div class="input-prepend input-group">
              <span>
                <button class="btn btn-sm" (click)="logout()">Logout</button>
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class ForbiddenComponent {
  constructor(
    private securityService: SecurityService,
    private router: Router) { }

  logout() {
    this.securityService.logout();
    this.router.navigate(['/login']);
  }
}

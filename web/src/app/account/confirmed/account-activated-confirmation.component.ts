import {Component} from "@angular/core";
import {Router} from "@angular/router";

@Component({
  template: `
    <div class="app flex-row align-items-center">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-md-6">
            <div class="clearfix">
              <h1 class="float-left display-3 mr-4">
                <i class="icon-check icons font-2xl d-block mt-4"></i>
              </h1>
              <h4 class="pt-3">Account activated!</h4>
              <p class="text-muted">You are now enabled to login</p>
            </div>
            <div class="input-prepend input-group">
              <button class="btn btn-sm btn-primary" (click)="backToLogin()">Back to login</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class AccountActivatedConfirmationComponent {

  constructor(private router: Router) { }

  backToLogin() {
    this.router.navigate(['/login']);
  }

}

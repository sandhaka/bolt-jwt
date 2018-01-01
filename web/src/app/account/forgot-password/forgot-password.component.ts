import {Component} from "@angular/core";
import {AccountService} from "../account.service";
import {ReactiveFormComponent} from "../../shared/base/reactiveForm.component";
import {FormBuilder, Validators} from "@angular/forms";
import {UtilityService} from "../../shared/utils.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  templateUrl: 'forgot-password.component.html'
})
export class ForgotPasswordComponent extends ReactiveFormComponent {

  isPending = false;

  constructor(
    private accountService: AccountService,
    private formBuilder: FormBuilder,
    private utils: UtilityService
  ) {
    super();

    // Form setup
    this.formErrors = {
      'email': '',
    };
    this.validationMessages = {
      'email': {
        'required': 'Required',
        'email': 'Must be a valid email address'
      }
    };
    this.form = this.formBuilder.group({
      'email': [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ]
    });

    // Trigger validation on form data change
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });

    this.onDataChanged();
  }

  submit() {

    this.isPending = true;

    const command = {
      email: this.form.value.email
    };

    this.accountService.forgotPassword(command).subscribe(
      () => {
        this.isPending = false;
        this.utils.openCustomModal(
          "Email sent",
          `An email has been sent to ${this.form.value.email} to reset the password`,
          "modal-info");
      },
      (error: HttpErrorResponse) => {
        this.isPending = false;
        this.utils.handleHttpError(error);
      }
    );
  }
}

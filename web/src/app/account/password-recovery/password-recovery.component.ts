import {ReactiveFormComponent} from "../../shared/base/reactiveForm.component";
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../account.service";
import {UtilityService} from "../../shared/utils.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Component, OnInit} from "@angular/core";

@Component({
  templateUrl: '/password-recovery.component.html'
})
export class PasswordRecoveryComponent extends ReactiveFormComponent implements OnInit {

  private authorizationCode: string;
  private userId: number;

  isPending = false;

  constructor(
    private formBuilder: FormBuilder,
    private utils: UtilityService,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private router: Router
  ) {
    super();

    // Form setup
    this.formErrors = {
      'password': '',
      'confirmPassword': ''
    };
    this.validationMessages = {
      'password': {
        'required': 'Required',
        'minlength': 'Minimum 6 character length'
      },
      'confirmPassword': {
        'required': 'Required',
        'minlength': 'Minimum 6 character length'
      }
    };
    this.form = this.formBuilder.group({
      'password': [
        '',
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ],
      'confirmPassword': [
        '',
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ]
    }, {validator: this.matchingPassword});

    // Trigger validation on form data change
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });

    this.onDataChanged();
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.authorizationCode = params['code'];
      this.userId = +params['userId'];
      if((/^$/).test(this.authorizationCode || '')) {
        this.router.navigate(['/login']);
      }
    });
  }

  /**
   * Send the reset password command
   */
  submit() {

    this.isPending = true;

    const formValue = this.form.value;

    // Build the command
    const command = {
      userId: this.userId,
      code: this.authorizationCode,
      password: formValue.password,
      confirmPassword: formValue.confirmPassword
    };

    this.accountService.resetPassword(command).subscribe(
      () => {
        this.isPending = false;
        this.utils.openCustomModal(
          "Reset success",
          "Your password has been reset successfully",
          "modal-success");
        this.router.navigate(['/login']);
      },
      (error: HttpErrorResponse) => {
        this.isPending = false;
        this.utils.handleHttpError(error);
      }
    );
  }

  matchingPassword(group: FormGroup) {
    const pass = group.value;
    return (pass.password === pass.confirmPassword) ? null : {
      invalid: true
    };
  }
}

import { Component, OnInit } from '@angular/core';
import {ReactiveFormComponent} from "../../shared/base/reactiveForm.component";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UtilityService} from "../../shared/utils.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AccountService} from "../account.service";
import {HttpErrorResponse} from "@angular/common/http";

// TODO: need a 'return url' parameter to redirect to the real application
@Component({
  selector: 'app-account-activation',
  templateUrl: './account-activation.component.html',
  styleUrls: ['./account-activation.component.scss']
})
export class AccountActivationComponent extends ReactiveFormComponent implements OnInit {

  private activationCode: string;

  // Form setup
  formErrors = {
    password: '',
    confirmPassword: ''
  };

  isPending = false;

  constructor(
    private formBuilder: FormBuilder,
    private utils: UtilityService,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private router: Router
  ) {
    super();

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
      this.activationCode = params['code'];
      if((/^$/).test(this.activationCode || '')) {
        this.router.navigate(['/login']);
      }
    });
  }

  /**
   * Send the activation request with the new password
   */
  submit() {

    this.isPending = true;

    const formValue = this.form.value;

    // Build the command
    const command = {
      code: this.activationCode,
      password: formValue.password,
      confirmPassword: formValue.confirmPassword
    };

    this.accountService.activate(command).subscribe(
      () => {
        this.isPending = false;
        this.router.navigate(['/account/confirmed']);
      },
      (error: HttpErrorResponse) => {
        this.isPending = false;
        this.utils.handleHttpError(error);
      }
    )
  }

  matchingPassword(group: FormGroup) {
    const pass = group.value;
    return (pass.password === pass.confirmPassword) ? null : {
      invalid: true
    }
  }
}

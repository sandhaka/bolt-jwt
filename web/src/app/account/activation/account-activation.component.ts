import { Component, OnInit } from '@angular/core';
import {ReactiveFormComponent} from "../../shared/base/reactiveForm.component";
import {BsModalService} from "ngx-bootstrap";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UtilityService} from "../../shared/utils.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AccountService} from "../account.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-account-activation',
  templateUrl: './account-activation.component.html',
  styleUrls: ['./account-activation.component.scss']
})
export class AccountActivationComponent extends ReactiveFormComponent implements OnInit {

  private activationCode: string;

  isPending = false;

  constructor(
    private formBuilder: FormBuilder,
    private modalService: BsModalService,
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
        'required': 'Required'
      },
      'confirmPassword': {
        'required': 'Required'
      }
    };
    this.form = this.formBuilder.group({
      'password': [
        '',
        Validators.required
      ],
      'confirmPassword': [
        '',
        Validators.required
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

  submit() {

    this.isPending = true;

    const formValue = this.form.value;

    const command = {
      code: this.activationCode,
      password: formValue.password,
      confirmPassword: formValue.confirmPassword
    };

    this.accountService.activate(command).subscribe(
      () => {
        this.isPending = false;
        // TODO: Navigate to confirmed page
        this.router.navigate(['/login']);
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
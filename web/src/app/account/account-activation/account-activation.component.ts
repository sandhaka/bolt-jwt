import { Component, OnInit } from '@angular/core';
import {ReactiveFormComponent} from "../../shared/base/reactiveForm.component";
import {BsModalService} from "ngx-bootstrap";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UtilityService} from "../../shared/utils.service";

@Component({
  selector: 'app-account-activation',
  templateUrl: './account-activation.component.html',
  styleUrls: ['./account-activation.component.scss']
})
export class AccountActivationComponent extends ReactiveFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private modalService: BsModalService,
    private utils: UtilityService
  ) {
    super();

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

  }

  submit() {

  }

  matchingPassword(group: FormGroup) {
    const pass = group.value;
    return (pass.password === pass.confirmPassword) ? null : {
      invalid: true
    }
  }
}

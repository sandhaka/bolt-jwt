import { Component } from '@angular/core';
import {GenericModalComponent} from "../../../shared/modals";
import {BsModalRef} from "ngx-bootstrap/modal";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Subject} from "rxjs/Subject";
import {ANIMATION_TYPES} from "ngx-loading";
import {HttpErrorResponse} from "@angular/common/http";
import {UsersService} from "../users.service";
import {UtilityService} from "../../../shared/utils.service";

@Component({
  selector: 'app-create-user-modal',
  templateUrl: './create-user-modal.component.html',
  styleUrls: ['./create-user-modal.component.scss']
})
export class CreateUserModalComponent extends GenericModalComponent {

  // On success user created event
  onCreated = new Subject<string>();

  // loading splash screen configuration
  loadingConfig = {
    backdropBorderRadius: '14px',
    animationType: ANIMATION_TYPES.wanderingCubes
  };

  isOnProcessing = false;

  //#region [ Form variables ]

  form: FormGroup;
  validationMessages = {
    'name': {
      'required': 'Required'
    },
    'surname': {
      'required': 'Required'
    },
    'username': {
      'required': 'Required'
    },
    'email': {
      'required': 'Required',
      'email': 'Must be a valid email'
    }
  };
  formErrors = {
    'name': '',
    'surname': '',
    'username': '',
    'email': ''
  };

  /**
   * Process the validation error messages
   * @param data
   */
  private onDataChanged(data?: any) {
    if (!this.form) {
      return;
    }

    const _form = this.form;

    for (const field in this.formErrors) {
      if (this.formErrors.hasOwnProperty(field)) {
        const control = _form.get(field);
        this.formErrors[field] = '';

        if (control && control.dirty && !control.valid) {
          const messages = this.validationMessages[field];
          for (const key in control.errors) {
            if (control.errors.hasOwnProperty(key)) {
              this.formErrors[field] += messages[key] + ' ';
            }
          }
        }
      }
    }
  }

  //#endregion

  constructor(
    public bsModalRef: BsModalRef,
    private formBuilder: FormBuilder,
    private usersService: UsersService,
    private utils: UtilityService,
  ) {
    super(bsModalRef);

    this.form = this.formBuilder.group({
      'name': [
        '',
        Validators.required
      ],
      'surname': [
        '',
        Validators.required
      ],
      'username': [
        '',
        Validators.required
      ],
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

  /**
   * Send the insert user command and handle the result
   */
  submit() {

    this.isOnProcessing = true;

    // Compose the insert command
    const command = {
      Name: this.form.value.name,
      Surname: this.form.value.surname,
      UserName: this.form.value.username,
      Email: this.form.value.email
    };

    // Send the command
    this.usersService.add(command).subscribe(
      () => {
        this.onCreated.next(command.Email);
        this.isOnProcessing = false;
      },
      (error: HttpErrorResponse) => {
        this.utils.handleHttpError(error);
        this.isOnProcessing = false;
      }
    );
  }
}

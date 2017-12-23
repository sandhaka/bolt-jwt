import { Component } from '@angular/core';
import {GenericModalComponent} from "../../../shared/modals";
import {BsModalRef} from "ngx-bootstrap/modal";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Subject} from "rxjs/Subject";

@Component({
  selector: 'app-create-user-modal',
  templateUrl: './create-user-modal.component.html',
  styleUrls: ['./create-user-modal.component.scss']
})
export class CreateUserModalComponent extends GenericModalComponent {

  onCreate = new Subject<UserCreateDto>();

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
    private formBuilder: FormBuilder
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

  submit() {
    console.log(this.form.value);

    const dto = new UserCreateDto();
    dto.email = this.form.value.email;
    dto.surname = this.form.value.surname;
    dto.username = this.form.value.username;
    dto.name = this.form.value.name;

    this.onCreate.next(dto);
  }

}

class UserCreateDto {
  name: string;
  surname: string;
  username: string;
  email: string;
}

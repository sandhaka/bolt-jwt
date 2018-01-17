import {Component} from "@angular/core";
import {GenericModalComponent} from "../../../shared/modals";
import {Subject} from "rxjs/Subject";
import {ANIMATION_TYPES} from "ngx-loading";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {BsModalRef} from "ngx-bootstrap/modal";
import {RolesService} from "../roles.service";
import {UtilityService} from "../../../shared/utils.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  templateUrl: './create-role-modal.component.html'
})
export class CreateRoleModalComponent extends GenericModalComponent {

  onCreated = new Subject<string>();

  // loading splash screen configuration
  loadingConfig = {
    backdropBorderRadius: '14px',
    animationType: ANIMATION_TYPES.wanderingCubes
  };

  isOnProcessing = false;

  //#region [ Form variables ]

  form: FormGroup;
  validationMessages= {
    'description': {
      'required': 'Required'
    }
  };
  formErrors = {
    'description': ''
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

  constructor(public bsModalRef: BsModalRef,
              private formBuilder: FormBuilder,
              private roleService: RolesService,
              private utils: UtilityService) {
    super(bsModalRef);

    this.form = this.formBuilder.group({
      'description': [
        '',
        Validators.required
      ]
    });

    // Trigger validation on form data change
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });

    this.onDataChanged();
  }

  /**
   * Send the insert role command and handle the result
   */
  submit() {

    this.isOnProcessing = true;

    // Compose the insert command
    const command = {
      Description: this.form.value.description
    };

    this.roleService.add(command).subscribe(
      () => {
        this.onCreated.next();
        this.isOnProcessing = false;
      },
      (error: HttpErrorResponse) => {
        this.utils.handleHttpError(error);
        this.isOnProcessing = false;
      }
    )
  }
}

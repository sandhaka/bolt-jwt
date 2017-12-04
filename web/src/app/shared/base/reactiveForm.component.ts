import {FormGroup} from "@angular/forms";
import {Component} from "@angular/core";

/**
 * Abstract reactive form component
 */
@Component({
  template: ''
})
export class ReactiveFormComponent {

  /**
   * Validation messages.
   * Setup this in the constructor of the component implementation. Add a list of validators code.
   *
   * for example:
   * this.validationMessages = {
   *  'username': {
   *    'required': 'Required'
   *  },
   *  ...
   * @type {{}}
   */
  public validationMessages = {};
  /**
   * Errors strings binding to the form.
   *
   * Setup as the previous.
   *
   * For example:
   * this.formErrors = {
   *   'username': '',
   *   'password': ''
   * };
   * @type {{}}
   */
  public formErrors = {};
  /**
   * Form object
   */
  public form: FormGroup;

  /**
   * Process the validation error messages
   * @param data
   */
  protected onDataChanged(data?: any) {
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
}

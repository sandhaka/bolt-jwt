import {Component, EventEmitter, Input, OnChanges, Output, SimpleChanges} from "@angular/core";
import {ReactiveFormComponent} from "../../../shared/base/reactiveForm.component";
import {RoleDto} from "../roleDto";
import {FormBuilder, Validators} from "@angular/forms";
import {UtilityService} from "../../../shared/utils.service";
import {RolesService} from "../roles.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-role-details',
  templateUrl: './role-details.component.html'
})
export class RoleDetailsComponent extends ReactiveFormComponent implements OnChanges {

  /**
   * Role
   */
  @Input('role') role: RoleDto;

  /**
   * On a successfully changes
   * @type {EventEmitter<any>}
   */
  @Output('changed') changed: EventEmitter<any> = new EventEmitter();

  formErrors = {
    description: ''
  };

  isPending = false;

  constructor(
    private formBuilder: FormBuilder,
    private utils: UtilityService,
    private roleService: RolesService
  ) {
    super();

    this.validationMessages = {
      'description': {
        'required': 'Required'
      }
    };
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

  saveRoleDetails() {

    this.isPending = true;

    const command = {
      Id: this.role.Id,
      Description: this.form.value.description
    };

    this.roleService.edit(command).subscribe(
      response => {
        this.isPending = false;
        this.changed.emit();
      },
      (errorResponse: HttpErrorResponse) => {
        this.utils.handleHttpError(errorResponse);
        this.isPending = false;
      }
    );
  }

  delete() {
    const deleteRoleCallback = function() {

      this.roleService.delete(this.role.Id).subscribe(
        () => {
          this.changed.emit();
        },
        (errorResponse: HttpErrorResponse) => this.utils.handleHttpError(errorResponse)
      );
    }
      .bind(this);

    // Call the delete service after confirmation
    this.utils.openConfirmModal(
      "Delete confirmation",
      `Are you sure to delete the role: ${this.role.Description}?`, "modal-warning",
      deleteRoleCallback
    );
  }

  /**
   * Update the form values on parent component selection changed
   * @param {SimpleChanges} changes
   */
  ngOnChanges(changes: SimpleChanges): void {
    this.resetForm();
  }

  private resetForm() {
    this.form.reset({
      description: this.role.Description
    });
  }
}

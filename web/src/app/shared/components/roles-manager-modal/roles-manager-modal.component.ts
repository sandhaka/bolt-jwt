import {Component} from "@angular/core";
import {GenericModalComponent} from "../../modals";
import {BsModalRef} from "ngx-bootstrap/modal";
import {ANIMATION_TYPES} from "ngx-loading";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UtilityService} from "../../utils.service";
import {AppEntity} from "../../common";
import {RolesManagerService} from "./roles-manager.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Role} from "./model/role";
import {AssignedRole} from "./model/assignedRole";

@Component({
  templateUrl: './roles-manager-modal.component.html',
  styleUrls: ['./roles-manager.component.scss'],
  providers: [RolesManagerService]
})
export class RolesManagerModalComponent extends GenericModalComponent {

  // loading splash screen configuration
  loadingConfig = {
    backdropBorderRadius: '14px',
    animationType: ANIMATION_TYPES.wanderingCubes
  };

  isOnProcessing = false;

  availableRoles: Role[] = [];
  assignedRoles: AssignedRole[] = [];

  serviceEntity: AppEntity;

  //#region [ Form variables ]

  form: FormGroup;
  validationMessages = {
    'availableRoles': {
      'required': 'Required'
    },
    'assignedRoles': {
      'required': 'Required'
    }
  };
  formErrors = {
    'availableRoles': '',
    'assignedRoles': ''
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
              private utils: UtilityService,
              private rolesManagerService: RolesManagerService) {
    super(bsModalRef);

    this.form = this.formBuilder.group({
      'availableRoles': [
        [''],
        Validators.required
      ],
      'assignedRoles': [
        [''],
        Validators.required
      ]
    });

    // Trigger validation on form data change
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });

    this.onDataChanged();
  }

  submit() {

  }

  load(entityId: number) {

    this.isOnProcessing = true;

    this.rolesManagerService.getAssignedRoles(this.serviceEntity, entityId).subscribe(
      (roles: AssignedRole[]) => {
        this.assignedRoles = roles;

        this.rolesManagerService.getRoles().subscribe(
          (allRoles: Role[]) => {

            // Filter role not assigned
            this.availableRoles = allRoles.filter((item: Role) => {
              return this.assignedRoles.filter((assigned: AssignedRole) => {
                return assigned.roleId === item.id;
              }).length === 0;
            });

            this.isOnProcessing = false;

          },
          (error: HttpErrorResponse) => {
            this.utils.handleHttpError(error);
            this.isOnProcessing = false;
          }
        );

      },
      (error: HttpErrorResponse) => {
        this.utils.handleHttpError(error);
        this.isOnProcessing = false;
      }
    );
  }
}

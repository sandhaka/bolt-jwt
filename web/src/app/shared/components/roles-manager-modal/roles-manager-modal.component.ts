import {Component} from "@angular/core";
import {GenericModalComponent} from "../../modals";
import {BsModalRef} from "ngx-bootstrap/modal";
import {ANIMATION_TYPES} from "ngx-loading";
import {UtilityService} from "../../utils.service";
import {AppEntity} from "../../common";
import {RolesManagerService} from "./roles-manager.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Role} from "./model/role";
import {AssignedRole} from "./model/assignedRole";

/**
 * Roles manager component
 * Manage roles for a user or group entity
 */
@Component({
  templateUrl: './roles-manager-modal.component.html',
  styleUrls: ['./roles-manager.component.scss'],
  providers: [RolesManagerService]
})
export class RolesManagerModalComponent extends GenericModalComponent {

  // -- Modal input ---

  // User or Group id
  entityId: number;

  // Define witch entity to serve
  serviceEntity: AppEntity;

  // -----------------

  // loading splash screen configuration
  loadingConfig = {
    backdropBorderRadius: '14px',
    animationType: ANIMATION_TYPES.wanderingCubes
  };

  // Touched flag
  touched = false;

  // Waiting for an application server response flag
  isOnProcessing = false;

  /**
   * Collection of available roles for the user
   * @type {any[]}
   */
  availableRoles: Role[] = [];
  selectedAvailableRoles: number[] = [];

  /**
   * Collection of user assigned roles
   * @type {any[]}
   */
  assignedRoles: AssignedRole[] = [];
  selectedAssignedRoles: number[];

  constructor(public bsModalRef: BsModalRef,
              private utils: UtilityService,
              private rolesManagerService: RolesManagerService) {
    super(bsModalRef);
  }

  /**
   * Save the current assigned roles to the user
   * Add the new roles and remove the unassigned
   */
  submit() {
    this.isOnProcessing = true;

    let command = null;

    // Command creation is on entity condition
    if(this.serviceEntity === AppEntity.User) {
      command = {
        UserId: this.entityId,
        Roles: this.assignedRoles
          .map(role => role.roleId)
      };
    } else if(this.serviceEntity === AppEntity.Group) {
      command = {
        GroupId: this.entityId,
        Roles: this.assignedRoles
          .map(role => role.roleId)
      };
    }

    this.rolesManagerService.assignRoles(this.serviceEntity, command).subscribe(
      () => {
        // Reload table on success
        this.load();
      },
      (error: HttpErrorResponse) => {
        this.utils.handleHttpError(error);
        this.isOnProcessing = false;
      }
    );
  }

  /**
   * Load routine
   * First, load the user assigned roles then, create a list of available roles for that user
   */
  load() {

    this.isOnProcessing = true;

    this.rolesManagerService.getAssignedRoles(this.serviceEntity, this.entityId).subscribe(
      (roles: AssignedRole[]) => {

        // User roles
        this.assignedRoles = roles;

        // Get all roles available in the sustem
        this.rolesManagerService.getRoles().subscribe(
          (allRoles: Role[]) => {

            // Subtract from all roles the assigned ones
            this.availableRoles = allRoles.filter((item: Role) => {
              return this.assignedRoles.filter((assigned: AssignedRole) => {
                return assigned.roleId === item.id;
              }).length === 0;
            });

            this.isOnProcessing = false;
            this.touched = false;

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

  /**
   * Move one or more roles from the available column to the assigned column
   */
  addToAssigned() {

    if(this.selectedAvailableRoles.length === 0) {
      return;
    }

    for(const id of this.selectedAvailableRoles) {
      // Move to the assigned roles list
      const role = this.availableRoles.find(i => i.id === id);
      this.assignedRoles.push({roleId: role.id, entityId: this.entityId, role: role.description});
      const indexToRemove = this.availableRoles.indexOf(role);
      this.availableRoles.splice(indexToRemove, 1);
    }
    this.selectedAvailableRoles = [];
    this.touched = true;
  }

  /**
   * Move one or more roles from the assigned column to the available column
   */
  removeFromAssigned() {

    if(this.selectedAssignedRoles.length === 0) {
      return;
    }

    for(const id of this.selectedAssignedRoles) {
      // Move to the available roles list
      const role = this.assignedRoles.find(i => i.roleId === id);
      this.availableRoles.push({id: role.roleId, description: role.role});
      const indexToRemove = this.assignedRoles.indexOf(role);
      this.assignedRoles.splice(indexToRemove, 1);
    }
    this.selectedAssignedRoles = [];
    this.touched = true;
  }
}

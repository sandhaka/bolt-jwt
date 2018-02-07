import {Component} from "@angular/core";
import {ANIMATION_TYPES} from "ngx-loading";
import {Group} from "./model/group";
import {AssignedGroup} from "./model/assignedGroup";
import {GenericModalComponent} from "../../../shared/modals";
import {BsModalRef} from "ngx-bootstrap/modal";
import {UtilityService} from "../../../shared/utils.service";
import {HttpErrorResponse} from "@angular/common/http";
import {GroupsManagerModalService} from "./groups-manager-modal.service";

@Component({
  templateUrl: './groups-manager-modal.component.html',
  styleUrls: ['./groups-manager-modal.component.scss']
})
export class GroupsManagerModalComponent extends GenericModalComponent {

  // --- Modal inputs:
  userId: number;
  // ----------------

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
   * Collection of available groups for the user
   * @type {any[]}
   */
  availableGroups: Group[] = [];
  selectedAvailableGroups: number[] = [];

  /**
   * Collection of user assigned groups
   * @type {any[]}
   */
  assignedGroups: AssignedGroup[] = [];
  selectedAssignedGroups: number[];

  constructor(public bsModalRef: BsModalRef,
              private utils: UtilityService,
              private groupsManagerService: GroupsManagerModalService) {
    super(bsModalRef);
  }

  /**
   * Load routine
   * First, load the user assigned groups then, create a list of available groups for that user
   */
  load() {

    this.isOnProcessing = true;

    this.groupsManagerService.getAssignedGroups(this.userId).subscribe(
      (groups: AssignedGroup[]) => {

        // User groups
        this.assignedGroups = groups;

        // Get all groups available in the sustem
        this.groupsManagerService.getGroups().subscribe(
          (allGroups: Group[]) => {

            // Subtract from all groups the assigned ones
            this.availableGroups = allGroups.filter((item: Group) => {
              return this.assignedGroups.filter((assigned: AssignedGroup) => {
                return assigned.groupId === item.id;
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
   * Save the current assigned groups to the user
   * Add the new groups and remove the unassigned
   */
  submit() {
    this.isOnProcessing = true;

    const command = {
      UserId: this.userId,
      Groups: this.assignedGroups
        .map(group => group.groupId)
    };

    this.groupsManagerService.assignGroups(command).subscribe(
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
   * Move one or more groups from the available column to the assigned column
   */
  addToAssigned() {
    if(this.selectedAvailableGroups.length === 0) {
      return;
    }

    for(const id of this.selectedAvailableGroups) {
      // Move to the assigned roles list
      const group = this.availableGroups.find(i => i.id === id);
      this.assignedGroups.push({groupId: group.id, userId: this.userId, groupName: group.description});
      const indexToRemove = this.availableGroups.indexOf(group);
      this.availableGroups.splice(indexToRemove, 1);
    }
    this.selectedAvailableGroups = [];
    this.touched = true;
  }

  /**
   * Move one or more groups from the assigned column to the available column
   */
  removeFromAssigned() {
    if(this.selectedAssignedGroups.length === 0) {
      return;
    }

    for(const id of this.selectedAssignedGroups) {
      // Move to the available roles list
      const group = this.assignedGroups.find(i => i.groupId === id);
      this.availableGroups.push({id: group.groupId, description: group.groupName});
      const indexToRemove = this.assignedGroups.indexOf(group);
      this.assignedGroups.splice(indexToRemove, 1);
    }
    this.selectedAssignedGroups = [];
    this.touched = true;
  }
}

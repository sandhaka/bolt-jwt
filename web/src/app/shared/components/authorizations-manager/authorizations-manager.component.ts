import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import {AuthorizationManagerService} from "./authorization-manager.service";
import {AppEntity} from "../../common";
import {HttpErrorResponse} from "@angular/common/http";
import {UtilityService} from "../../utils.service";
import {EntityData} from "./model/entity";
import {Authorization, AuthorizationDefinition} from "./model/authorizations";
import {ANIMATION_TYPES} from "ngx-loading";

/**
 * This component manage authorization for a user, role or group
 */
@Component({
  selector: 'app-authorizations-manager',
  templateUrl: './authorizations-manager.component.html',
  styleUrls: ['./authorizations-manager.component.scss'],
  providers: [AuthorizationManagerService]
})
export class AuthorizationsManagerComponent implements OnChanges {

  /**
   * Witch type of entity
   */
  @Input('entity') entity: AppEntity;

  /**
   * Entity data related
   */
  @Input('entityData') entityData: EntityData;

  /**
   * A list a available authorization to assign
   */
  availableAuthorizations: AuthorizationDefinition[];
  /**
   * All authorization defined in the system
   * @type {null}
   */
  authorizationsDefinition: AuthorizationDefinition[] = null;

  // Flags
  addingDialog = false;
  isOnProcessing = false;

  // loading splash screen configuration
  loadingConfig = {
    backdropBorderRadius: '14px',
    animationType: ANIMATION_TYPES.wanderingCubes
  };

  constructor(
    private authorizationManagerService: AuthorizationManagerService,
    private utils: UtilityService) {

  }

  /**
   * On changes, hide the 'adding' dialog and load the authorization for the current entity
   * @param {SimpleChanges} changes
   */
  ngOnChanges(changes: SimpleChanges): void {
    this.addingDialog = false;
    this.availableAuthorizations = [];
    this.load();
  }

  /**
   * Load authorization for the current entity
   */
  load() {

    this.isOnProcessing = true;

    if(this.entity === AppEntity.Role) {
      console.error("Not yet implemented");
    } else if(this.entity === AppEntity.User) {
      this.authorizationManagerService.getUserAuthorizations(this.entityData.Id)
        .subscribe((authorizations: Authorization[]) => {
          // Assign the authorizations
          this.entityData.AuthorizationsList = authorizations;
          this.isOnProcessing = false;
          },
          (errorResponse: HttpErrorResponse) => {
          this.utils.handleHttpError(errorResponse);
          this.isOnProcessing = false;
        });
    }
  }

  /**
   * Remove the authorizations
   */
  removeAuthorizations() {

    // Collects from the 'checked status'
    const authToRem = this.collectsAuthorizationToRemove();

    if(authToRem.length > 0) {

      const command = {
        userId: this.entityData.Id,
        authorizations: authToRem
      };

      this.isOnProcessing = true;

      this.authorizationManagerService.removeUserAuthorization(command).subscribe(
        response => {
          // Reload on success
          this.load();
        },
        (error: HttpErrorResponse) => {
          this.utils.handleHttpError(error);
          this.isOnProcessing = false;
        }
      );
    }
  }

  /**
   * Allow to select a list of authorizations to add and save the selected (second trigger)
   */
  adding() {
    if (!this.addingDialog) {

      this.showAddingDialog();

    } else {

      this.saveAndHideAddingDialog();

    }
  }

  private collectsAvailableDefinitions() {
    this.availableAuthorizations = [];
    for (const authorizationDefinition of this.authorizationsDefinition) {
      if (!this.entityData.AuthorizationsList.find(i => i.authId === authorizationDefinition.id)) {
        this.availableAuthorizations.push(authorizationDefinition);
      }
    }
  }

  private collectsAuthorizationToAdd(): AuthorizationDefinition[] {
    const collection = [];
    for(const a of this.availableAuthorizations) {
      if(a.checked === true) {
        collection.push(a.id);
      }
    }
    return collection;
  }

  private collectsAuthorizationToRemove(): AuthorizationDefinition[] {
    const collection = [];
    for(const a of this.entityData.AuthorizationsList) {
      if(a.checked === true) {
        collection.push(a.entityAuthId);
      }
    }
    return collection;
  }

  private showAddingDialog() {
    this.addingDialog = true;

    if(!this.authorizationsDefinition) {

      this.isOnProcessing = true;

      this.authorizationManagerService.getAuthorizationsDefinition().subscribe(
        (authorizations: AuthorizationDefinition[]) => {

          this.authorizationsDefinition = authorizations;

          this.collectsAvailableDefinitions();

          this.isOnProcessing = false;
        },
        (error: HttpErrorResponse) => {
          this.utils.handleHttpError(error);
          this.isOnProcessing = false;
        }
      );
    } else {
      this.collectsAvailableDefinitions();
    }
  }

  private saveAndHideAddingDialog() {
    if(this.entity === AppEntity.User) {

      const authToAdd = this.collectsAuthorizationToAdd();

      if(authToAdd.length > 0) {

        this.isOnProcessing = true;

        const command = {
          UserId: this.entityData.Id,
          Authorizations: authToAdd
        };

        this.authorizationManagerService.addUserAuthorizations(command).subscribe(
          response => {
            // Reload on success
            this.load();
          },
          (error: HttpErrorResponse) => {
            this.utils.handleHttpError(error);
            this.isOnProcessing = false;
          }
        );
      }
    } else if(this.entity === AppEntity.Role) {
      console.error("Not yet implemented");
    }

    this.addingDialog = false;
  }
}
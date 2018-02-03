import {Component} from "@angular/core";
import {GenericModalComponent} from "../../../shared/modals";
import {UtilityService} from "../../../shared/utils.service";
import {BsModalRef} from "ngx-bootstrap/modal";
import {RolesService} from "../roles.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-view-role-usage-modal',
  templateUrl: './view-role-usage-modal.component.html'
})
export class ViewRoleUsageModalComponent extends GenericModalComponent {

  users: string[];
  groups: string[];

  roleId: number;

  constructor(
    public bsModalRef: BsModalRef,
    private rolesService: RolesService,
    private utils: UtilityService
  ) {
    super(bsModalRef);
  }

  load() {
    this.rolesService.getUsage(this.roleId).subscribe(
      (results: any) => {
        this.users = results.users;
        this.groups = results.groups;
      },
      (error: HttpErrorResponse) => this.utils.handleHttpError(error)
    );
  }
}

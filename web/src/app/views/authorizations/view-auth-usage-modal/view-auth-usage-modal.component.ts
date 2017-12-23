import {Component} from '@angular/core';
import {GenericModalComponent} from "../../../shared/modals";
import {BsModalRef} from "ngx-bootstrap/modal";
import {AuthorizationsService} from "../authorizations.service";
import {HttpErrorResponse} from "@angular/common/http";
import {UtilityService} from "../../../shared/utils.service";

@Component({
  selector: 'app-view-auth-usage-modal',
  templateUrl: './view-auth-usage-modal.component.html',
  styleUrls: ['./view-auth-usage-modal.component.scss']
})
export class ViewAuthUsageModalComponent extends GenericModalComponent {

  users: string[];
  roles: string[];

  authId: number;

  constructor(
    public bsModalRef: BsModalRef,
    private authService: AuthorizationsService,
    private utils: UtilityService
  ) {
    super(bsModalRef);
  }

  load(): void {
    this.authService.getUsage(this.authId).subscribe(
      (results: any) => {
        this.users = results.users;
        this.roles = results.roles;
      },
      (error: HttpErrorResponse) => this.utils.handleHttpError(error)
    );
  }

}

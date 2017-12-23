import { Component } from '@angular/core';
import {GenericModalComponent} from "../../../shared/modals";
import {BsModalRef} from "ngx-bootstrap/modal";
import {Subject} from "rxjs/Subject";

@Component({
  selector: 'app-create-auth-modal',
  templateUrl: './create-auth-modal.component.html',
  styleUrls: ['./create-auth-modal.component.scss']
})
export class CreateAuthModalComponent extends GenericModalComponent {

  onCreate = new Subject<string>();

  authorizationName = '';

  /**
   * A valid authorization name is 3 to 12 letters length
   * @type {string}
   */
  private pattern = "[a-zA-Z]{3,12}";

  constructor(public bsModalRef: BsModalRef) {
    super(bsModalRef);
  }

  submit() {
    this.onCreate.next(this.authorizationName);
  }
}

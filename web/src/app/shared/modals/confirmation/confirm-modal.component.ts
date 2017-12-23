import {Component} from "@angular/core";
import {BsModalRef} from "ngx-bootstrap/modal";
import {GenericModalComponent} from "../generic/generic-modal.component";
import {Subject} from "rxjs/Subject";

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html'
})
export class ConfirmModalComponent extends GenericModalComponent {

  onClose = new Subject<boolean>();

  constructor(public bsModalRef: BsModalRef) {
    super(bsModalRef);
  }

  onConfirmation() {
    this.onClose.next(true);
    this.bsModalRef.hide();
  }

  onCancel() {
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

  onExit() {
    this.onClose.next(false);
    this.bsModalRef.hide();
  }
}

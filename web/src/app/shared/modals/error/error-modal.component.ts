import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html'
})
export class ErrorModalComponent {

  errorCode: number;
  detailedErrorDescr: string;
  errorDescr: string;

  errorDetailsAccordionOpened: boolean;

  constructor(public bsModalRef: BsModalRef) { }

  toggleAccordion() {
    this.errorDetailsAccordionOpened = !this.errorDetailsAccordionOpened
  }
}

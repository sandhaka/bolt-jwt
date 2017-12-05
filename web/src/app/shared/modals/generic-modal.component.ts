import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-modal',
  templateUrl: './generic-modal.component.html'
})
export class GenericModalComponent {

  modalText: string;
  modalTitle: string;
  modalClass: string;

  constructor(public bsModalRef: BsModalRef) { }
}

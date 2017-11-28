import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html'
})
export class ModalComponent {

  modalText: string;
  modalTitle: string;
  modalClass: string;

  constructor(public bsModalRef: BsModalRef) { }
}

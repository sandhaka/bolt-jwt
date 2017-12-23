import { Component, OnInit } from '@angular/core';
import {GenericModalComponent} from "../../../shared/modals";
import {BsModalRef} from "ngx-bootstrap/modal";

@Component({
  selector: 'app-create-user-modal',
  templateUrl: './create-user-modal.component.html',
  styleUrls: ['./create-user-modal.component.scss']
})
export class CreateUserModalComponent extends GenericModalComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef) {
    super(bsModalRef);
  }

  ngOnInit() {
  }

}

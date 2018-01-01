import {Injectable} from '@angular/core';
import {HttpErrorResponse} from "@angular/common/http";
import {ConfirmModalComponent, GenericModalComponent} from "./modals";
import {BsModalService} from "ngx-bootstrap";

@Injectable()
export class UtilityService {

  /**
   * Decode Json web token content
   * @param {string} token
   * @returns {JSON}
   */
  static decodeToken(token: string): any {

    const base64 = token.split('.')[1]
      .replace('-', '+')
      .replace('_', '/');

    return JSON.parse(window.atob(base64));
  }

  constructor(private modalService: BsModalService) { }

  /**
   * Handle a http error
   * @param {HttpErrorResponse} errorResponse
   */
  handleHttpError(errorResponse: HttpErrorResponse) {
    const errorDetails = errorResponse.error && errorResponse.error.Message ?
      errorResponse.error.Message :
      errorResponse.message;
    this.openCustomModal('Error', `${errorResponse.statusText}: ${errorDetails}`, 'modal-danger');
  }

  /**
   * Open a confirmation modal
   * @param {string} title
   * @param {string} body
   * @param {string} cssClass
   * @param {() => void} callback
   */
  openConfirmModal(title: string, body: string, cssClass: string, callback: () => void) {
    const bsConfirmModalRef = this.modalService.show(ConfirmModalComponent);
    bsConfirmModalRef.content.modalTitle = title;
    bsConfirmModalRef.content.modalClass = cssClass;
    bsConfirmModalRef.content.modalText = body;
    bsConfirmModalRef.content.onClose.subscribe(result => {
      if(result === true) {
        callback();
      }
    });
  }

  /**
   * Open a generic modal
   * @param {string} title
   * @param {string} body
   * @param {string} cssClass
   */
  openCustomModal(title: string, body: string, cssClass: string) {
    const bsModalRef = this.modalService.show(GenericModalComponent);
    bsModalRef.content.modalTitle = title;
    bsModalRef.content.modalClass = cssClass;
    bsModalRef.content.modalText = body;
  }
}

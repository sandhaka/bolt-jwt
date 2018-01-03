import {Injectable} from '@angular/core';
import {HttpErrorResponse} from "@angular/common/http";
import {ConfirmModalComponent, GenericModalComponent} from "./modals";
import {BsModalService} from "ngx-bootstrap";
import {ErrorModalComponent} from "./modals/error/error-modal.component";

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
    if(typeof(errorResponse.error) !== "object") {
      const descr = errorResponse.statusText != null ? errorResponse.statusText : errorResponse.message;
      const detailedDescr = errorResponse.statusText + '\n\n' + errorResponse.message;
      this.openErrorModal(errorResponse.status, descr, detailedDescr);
    } else {
      const descr = errorResponse.error.Message;
      const detailedDescr = errorResponse.error.Details + '\n\n' + "StackTrace:" + '\n' + errorResponse.error.StackTrace;
      this.openErrorModal(errorResponse.error.StatusCode, descr, detailedDescr);
    }
  }

  openErrorModal(errorCode: number, errorDescr: string, detailedErrorDescr: string) {
    const bsErrorModalRef = this.modalService.show(ErrorModalComponent);
    bsErrorModalRef.content.errorDescr = errorDescr;
    bsErrorModalRef.content.detailedErrorDescr = detailedErrorDescr;
    bsErrorModalRef.content.errorCode = errorCode;
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

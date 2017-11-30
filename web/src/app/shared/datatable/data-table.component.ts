import {Component, Input, OnInit} from '@angular/core';
import {Page} from "./model/page";
import {Observable} from "rxjs/Observable";
import {PagedData} from "./model/paged-data";
import {BsModalRef, BsModalService} from "ngx-bootstrap";
import {ModalComponent} from "../modals/modal.component";
import {HttpErrorResponse} from "@angular/common/http";

/**
 *  ngx-datatable wrapper component
 */
@Component({
  selector: 'app-data-table',
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.scss']
})
export class DataTableComponent<T> implements OnInit {

  /**
   *  Column names
   */
  @Input('columnNames') columnNames: Array<any>;

  /**
   *  A service function that return an observable with the paged data
   */
  @Input('getData') getData: (params: any) => Observable<any>;

  /**
   *  Number of record for each page
   */
  @Input('pageSize') pageSize: number;

  private bsModalRef: BsModalRef;

  page = new Page();
  rows = new Array<T>();
  isLoading = false;

  constructor(private bsModalService: BsModalService) {
    this.page.pageNumber = 0;
  }

  ngOnInit() {
    // Init load
    this.page.size = this.pageSize;
    this.load({offset: 0});
  }

  /**
   * datatable callback to retrieve data
   * @param pageInfo
   */
  load(pageInfo) {

    this.isLoading = true;
    this.page.pageNumber = pageInfo.offset;

    this.getData(this.page).subscribe((pagedData: PagedData<T>) => {
      this.page = pagedData.page;
      this.rows = pagedData.data;
        this.isLoading = false;
    },
      (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.openModal('Error', `${error.statusText}: ${error.message}`, 'modal-danger');
      });
  }

  /**
   * Open a modal dialog
   * @param {string} title
   * @param {string} body
   * @param {string} cssClass
   */
  private openModal(title: string, body: string, cssClass: string) {
    this.bsModalRef = this.bsModalService.show(ModalComponent);
    this.bsModalRef.content.modalTitle = title;
    this.bsModalRef.content.modalClass = cssClass;
    this.bsModalRef.content.modalText = body;
  }
}

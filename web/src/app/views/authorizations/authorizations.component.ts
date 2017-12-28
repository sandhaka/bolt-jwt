import { Component, OnInit } from '@angular/core';
import {PagedData} from "../../shared/datatable/model/paged-data";
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs/Observable";
import {AuthorizationsService} from "./authorizations.service";
import {DataTableService} from "../../shared/datatable/data-table.service";
import {AuthorizationDto} from "./authDto";
import {HttpErrorResponse } from "@angular/common/http";
import {UtilityService} from "../../shared/utils.service";
import {BsModalService} from "ngx-bootstrap";
import {CreateAuthModalComponent} from "./create-auth-modal/create-auth-modal.component";
import {ViewAuthUsageModalComponent} from "./view-auth-usage-modal/view-auth-usage-modal.component";

@Component({
  selector: 'app-authorizations',
  templateUrl: './authorizations.component.html',
  styleUrls: ['./authorizations.component.scss'],
  providers: [DataTableService]
})
export class AuthorizationsComponent implements OnInit {

  columnNames = [
    {name: 'Name'}
  ];

  /**
   * Filters object for data table component
   * @type {any[]}
   */
  filters = [
    {label:'Name', type: 'string', name: 'name', cssClass: 'col-sm-3'}
  ];

  filtersAccordionOpened = false;

  /**
   * Service callbacks ref
   */
  getDataFunc: Function;
  onSelectFunc: Function;

  /**
   * Keep a copy of the item to working on
   */
  selectedAuth: AuthorizationDto;

  constructor(
    private authorizationsService: AuthorizationsService,
    private utils: UtilityService,
    private dataTableService: DataTableService,
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    // Bind
    this.getDataFunc = this.getData.bind(this);
    this.onSelectFunc = this.onSelect.bind(this);
  }

  /**
   * The service callback
   * @param params
   * @returns {Observable<PagedData<any>>}
   */
  getData(params: Page): Observable<PagedData<any>> {
    return this.authorizationsService.getAuthorizationsPaged(params);
  }

  /**
   * Auth. selection handler
   * Copy the selected auth. into a local dto
   * @param {any} selected
   */
  onSelect({selected}) {
    const selectedAuth = selected[0];
    this.selectedAuth = new AuthorizationDto();
    this.selectedAuth.Id = selectedAuth.id;
    this.selectedAuth.Name = selectedAuth.name;
  }

  /**
   * Open/Close filters accordion
   */
  toggleAccordion() {
    this.filtersAccordionOpened = !this.filtersAccordionOpened
  }

  /**
   * Request deletion for an authorization
   */
  delete() {

    const deleteAuthCallback = function() {

      this.authorizationsService.delete(this.selectedAuth.Id).subscribe(
        () => {
          this.dataTableService.invokeRowDelete();
        },
        (errorResponse: HttpErrorResponse) => this.utils.handleHttpError(errorResponse)
      );
    }.bind(this);

    // Call the delete service after confirmation
    this.utils.openConfirmModal(
      "Delete confirmation",
      `Are you sure to delete the authorization: ${this.selectedAuth.Name}?`, "modal-warning",
      deleteAuthCallback
    );
  }

  /**
   * Open a modal to add a new authorization,
   * on submit send the creation command and handle the response
   */
  addNew() {
    const bsAuthCreationModal = this.modalService.show(CreateAuthModalComponent);
    bsAuthCreationModal.content.modalTitle = "New authorization";
    bsAuthCreationModal.content.modalCss = "modal-info";
    bsAuthCreationModal.content.onCreated.subscribe(name => {
      const authInsertCommand = {
        Name: name
      };

      this.authorizationsService.create(authInsertCommand).subscribe(
        () => {
          bsAuthCreationModal.hide();
          this.dataTableService.invokeReload();
        },
        (errorResponse: HttpErrorResponse) => this.utils.handleHttpError(errorResponse)
      )
    });
  }

  /**
   * Show authorization usage
   */
  viewUsage() {
    const bsAuthUsageModal = this.modalService.show(ViewAuthUsageModalComponent, );
    bsAuthUsageModal.content.authId = this.selectedAuth.Id;
    bsAuthUsageModal.content.modalTitle = "Authorization usage";
    bsAuthUsageModal.content.modalCss = "modal-info";
    bsAuthUsageModal.content.load();
  }
}



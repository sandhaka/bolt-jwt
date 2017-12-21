import { Component, OnInit } from '@angular/core';
import {PagedData} from "../../shared/datatable/model/paged-data";
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs/Observable";
import {AuthorizationsService} from "./authorizations.service";
import {DataTableService} from "../../shared/datatable/data-table.service";
import {AuthorizationDto} from "./authDto";

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

  constructor(private authorizationsService: AuthorizationsService) { }

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
}

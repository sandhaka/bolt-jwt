import {Component, Input, OnInit} from '@angular/core';
import {Page} from "./model/page";
import {Observable} from "rxjs";
import {PagedData} from "./model/paged-data";
import {HttpErrorResponse} from "@angular/common/http";
import {DataTableService} from "./data-table.service";
import {UtilityService} from "../utils.service";

/**
 *  ngx-datatable wrapper component
 */
@Component({
  selector: 'app-data-table',
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.scss']
})
export class DataTableComponent implements OnInit {

  /**
   * Column names, are a list of object like: {name: 'column-name'}
   */
  @Input('columnNames') columnNames: Array<any>;

  /**
   * A service function that return an observable with the paged data
   */
  @Input('getData') getData: (params: Page) => Observable<PagedData<any>>;

  /**
   * Item selection handler
   */
  @Input('select') select: (selected: any) => void;

  /**
   * Number of record for each page
   */
  @Input('pageSize') pageSize: number;

  /**
   * Page model
   * @type {Page}
   */
  page = new Page();

  /**
   * Where the data is stored
   * @type {any[]}
   */
  rows = [];

  /**
   * Loading flag
   * @type {boolean}
   */
  isLoading = false;

  private filters: any[];

  constructor(
    private utils: UtilityService,
    private dataTableService: DataTableService) {

    this.page.pageNumber = 0;
    this.subscribeToTableEvents();
  }

  ngOnInit() {
    // Initial load
    this.page.size = this.pageSize;
    this.load({ offset: 0 });
  }

  /**
   * datatable callback to retrieve data from server
   * @param pageInfo
   */
  load(pageInfo) {

    this.isLoading = true;
    this.page.pageNumber = pageInfo.offset;
    this.page.filters = this.filters;

    this.getData(this.page).subscribe(
      (pagedData: PagedData<any>) => {
        this.page = pagedData.page;
        this.rows = pagedData.data;
        this.isLoading = false;
    },
      (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.utils.handleHttpError(error);
      });
  }

  private subscribeToTableEvents() {

    // Row edited
    this.dataTableService.rowEdited$.subscribe(() => {
      // Reload the page
      this.load({offset: this.page.pageNumber});
    });

    // Row deleted
    this.dataTableService.rowDelete$.subscribe(() => {
      this.load({offset: this.page.pageNumber});
    });

    // Reload request
    this.dataTableService.reload$.subscribe(() => {
      this.load({offset: this.page.pageNumber});
    });

    // Apply filters
    this.dataTableService.applyFilters$.subscribe(filters => {
      this.filters = filters;
      this.load({ offset: 0 });
    });
  }
}

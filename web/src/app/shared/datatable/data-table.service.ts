import {Injectable} from "@angular/core";
import {Subject} from "rxjs/Subject";

@Injectable()
export class DataTableService {

  private rowEdit = new Subject<any>();

  private applyFilters = new Subject<any>();

  rowEdited$ = this.rowEdit.asObservable();
  applyFilters$ = this.applyFilters.asObservable();

  /**
   * Notify to the subscribers that a row has been edited
   * @param row: edited object
   */
  invokeRowEdit(row) {
    this.rowEdit.next(row);
  }

  /**
   * Notify to the subscribers a request to apply the filters
   * @param filters: Filters selections
   */
  invokeApplyFilters(filters: any[]) {
    this.applyFilters.next(filters);
  }
}

import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable()
export class DataTableService {

  private rowEdit = new Subject<any>();
  private applyFilters = new Subject<any>();
  private rowDelete = new Subject<any>();
  private reload = new Subject<any>();

  rowEdited$ = this.rowEdit.asObservable();
  applyFilters$ = this.applyFilters.asObservable();
  rowDelete$ = this.rowDelete.asObservable();
  reload$ = this.reload.asObservable();

  /**
   * Notify to the subscribers that a row has been edited
   */
  invokeRowEdit() {
    this.rowEdit.next();
  }

  /**
   * Notify to the subscribers a request to apply the filters
   * @param filters: Filters selections
   */
  invokeApplyFilters(filters: any[]) {
    this.applyFilters.next(filters);
  }

  /**
   * Notify to the subscribers a item delete request
   */
  invokeRowDelete() {
    this.rowDelete.next();
  }

  /**
   * Invoke a generic reload
   */
  invokeReload() {
    this.reload.next();
  }
}

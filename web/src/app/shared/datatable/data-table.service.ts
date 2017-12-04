import {Injectable} from "@angular/core";
import {Subject} from "rxjs/Subject";

@Injectable()
export class DataTableService {

  private rowEdited = new Subject<any>();

  rowEdited$ = this.rowEdited.asObservable();

  editRow(row) {
    this.rowEdited.next(row);
  }
}

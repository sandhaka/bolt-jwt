import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Page} from "../../shared/datatable/model/page";
import {PagedData} from "../../shared/datatable/model/paged-data";

@Injectable()
export class TokenLogsService {

  constructor(private httpClient: HttpClient) { }

  getPaged(params: Page): Observable<PagedData<any>> {

    // Combine query
    const url =
      `/api/v1/tokenlogs?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    return this.httpClient.get<PagedData<any>>(url);
  }

}

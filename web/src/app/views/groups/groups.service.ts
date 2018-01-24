import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs/Observable";

@Injectable()
export class GroupsService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get paged groups
   * @param {Page} params Pagination and filters parameters
   * @returns {Observable<PagedData<any>>}
   */
  getGroupsPaged(params: Page): Observable<PagedData<any>> {

    // Combine query
    let url =
      `/api/v1/group/paged?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    // Send filters as json stringify object
    if(params.filters && params.filters.length > 0) {
      url += `&filters=${JSON.stringify(params.filters)}`;
    }

    return this.httpClient.get<PagedData<any>>(url);
  }

  edit(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/group/update', command);
  }

  delete(id: number): Observable<any> {
    return this.httpClient.delete(`/api/v1/group?id=${id}`);
  }

  add(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/group', command);
  }
}

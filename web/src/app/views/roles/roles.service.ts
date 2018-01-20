import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";

@Injectable()
export class RolesService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get paged roles
   * @param {Page} params Pagination and filters parameters
   * @returns {Observable<PagedData<any>>}
   */
  getRolesPaged(params: Page): Observable<PagedData<any>> {

    // Combine query
    let url =
      `/api/v1/role/paged?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    // Send filters as json stringify object
    if(params.filters && params.filters.length > 0) {
      url += `&filters=${JSON.stringify(params.filters)}`;
    }

    return this.httpClient.get<PagedData<any>>(url);
  }

   edit(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/role/update', command);
   }

   delete(id: number): Observable<any> {
    return this.httpClient.delete(`/api/v1/role?id=${id}`);
   }

   add(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/role', command);
   }
}

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

  /**
   * Update a role
   * @param command
   * @returns {Observable<any>}
   */
  edit(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/role/update', command);
  }

  /**
   * Delete a role
   * @param {number} id
   * @returns {Observable<any>}
   */
  delete(id: number): Observable<any> {
    return this.httpClient.delete(`/api/v1/role?id=${id}`);
  }

  /**
   * Add a new role
   * @param command
   * @returns {Observable<any>}
   */
  add(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/role', command);
  }

  /**
   * Query role usage
   * @param {number} id
   * @returns {Observable<any>} User and group list
   */
  getUsage(id: number): Observable<any> {
    return this.httpClient.get(`/api/v1/role/usage?id=${id}`);
  }
}

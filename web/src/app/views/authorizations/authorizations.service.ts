import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs";
import {PagedData} from "../../shared/datatable/model/paged-data";

@Injectable()
export class AuthorizationsService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get paged authorizations
   * @param {Page} params Pagination and filters parameters
   * @returns {Observable<PagedData<any>>}
   */
  getAuthorizationsPaged(params: Page): Observable<PagedData<any>> {

    // Combine query
    let url =
      `/api/v1/authorization/all?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    // Send filters as json stringify object
    if(params.filters && params.filters.length > 0) {
      url += `&filters=${JSON.stringify(params.filters)}`;
    }

    return this.httpClient.get<PagedData<any>>(url);
  }

  /**
   *
   * @param {number} id
   * @returns {Observable<any>}
   */
  delete(id: number): Observable<any> {
    return this.httpClient.delete(`/api/v1/authorization?id=${id}`);
  }

  /**
   *
   * @param authInsertCommand
   * @returns {Observable<any>}
   */
  create(authInsertCommand: any): Observable<any> {
    return this.httpClient.post('/api/v1/authorization', authInsertCommand);
  }

  /**
   * Return a list of users and roles that use the authorization
   * @param {number} id
   * @returns {Observable<any>}
   */
  getUsage(id: number): Observable<any> {
    return this.httpClient.get(`/api/v1/authorization/usage?id=${id}`);
  }
}

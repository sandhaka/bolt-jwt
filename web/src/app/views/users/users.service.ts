import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {Page} from "../../shared/datatable/model/page";

@Injectable()
export class UsersService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get users
   * @param {Page} params Pagination and filters parameters
   * @returns {Observable<PagedData<any>>}
   */
  getUsersPaged(params: Page): Observable<PagedData<any>> {

    // Combine query
    let url =
      `/api/v1/user/all?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    // Send filters as json stringify object
    if(params.filters && params.filters.length > 0) {
      url += `&filters=${JSON.stringify(params.filters)}`;
    }

    return this.httpClient.get<PagedData<any>>(url);
  }

  /**
   * Send a user edit command
   * @param dto command
   * @returns {Observable<any>}
   */
  saveUserDetails(dto: any): Observable<any> {
    return this.httpClient.post('/api/v1/user/update', dto);
  }
}

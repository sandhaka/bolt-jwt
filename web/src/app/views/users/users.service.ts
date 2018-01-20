import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {Page} from "../../shared/datatable/model/page";

@Injectable()
export class UsersService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get paged users
   * @param {Page} params Pagination and filters parameters
   * @returns {Observable<PagedData<any>>}
   */
  getUsersPaged(params: Page): Observable<PagedData<any>> {

    // Combine query
    let url =
      `/api/v1/user/paged?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    // Send filters as json stringify object
    if(params.filters && params.filters.length > 0) {
      url += `&filters=${JSON.stringify(params.filters)}`;
    }

    return this.httpClient.get<PagedData<any>>(url);
  }

  /**
   * Create a new user
   * @param insertCommand
   * @returns {Observable<any>}
   */
  add(insertCommand: any): Observable<any> {
    return this.httpClient.post('/api/v1/user', insertCommand);
  }

  /**
   * User edit
   * @param editCommand command
   * @returns {Observable<any>}
   */
  edit(editCommand: any): Observable<any> {
    return this.httpClient.post('/api/v1/user/update', editCommand);
  }

  /**
   * Delete a user
   * @param id
   * @returns {Observable<any>}
   */
  delete(id: number): Observable<any> {
    return this.httpClient.delete(`/api/v1/user?id=${id}`);
  }

  /**
   * Trigger a user password edit request
   * @param passwordRecoveryCommand
   * @returns {Observable<Object>}
   */
  triggerPasswordEdit(passwordRecoveryCommand: any) {
    return this.httpClient.post('/api/v1/account/password-recovery', passwordRecoveryCommand);
  }
}

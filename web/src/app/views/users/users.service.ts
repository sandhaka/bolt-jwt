import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {Page} from "../../shared/datatable/model/page";

@Injectable()
export class UsersService {

  constructor(private httpClient: HttpClient) { }

  getUsersPaged(params: Page): Observable<PagedData<any>> {

    // Combine query
    const url =
      `/api/v1/user/all?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    return this.httpClient.get<PagedData<any>>(url);
  }

  saveUserDetails(dto: any): Observable<any> {
    return this.httpClient.post('/api/v1/user/update', dto);
  }
}

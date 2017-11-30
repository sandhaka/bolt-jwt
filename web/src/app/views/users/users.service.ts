import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {AuthenticationService} from "../../security/authentication.service";

@Injectable()
export class UsersService {

  constructor(private httpClient: HttpClient, private authenticationService: AuthenticationService) { }

  getUsersPaged(params: any): Observable<PagedData<any>> {

    // Combine query
    const url =
      `/api/v1/user/all?pageNumber=${params.pageNumber}`+
      `&size=${params.size}`;

    return this.httpClient.get<PagedData<any>>(url,  {
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + this.authenticationService.token) // TODO: Move to a http interceptor
    });
  }
}

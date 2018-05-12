import { Injectable } from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse} from '@angular/common/http';
import { Observable } from 'rxjs';

// Import a "1 year valid" token
import {DevToken} from "./fake-token";
import {FakeUserPagedData} from "./fake-user-paged-data";

@Injectable()
export class MockHttpInterceptor implements HttpInterceptor {

  constructor() { }

  /**
   * Mock backend requests
   * @param {HttpRequest<any>} request
   * @param {HttpHandler} next
   * @returns {Observable<HttpEvent<any>>}
   */
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const url: string = request.url;
    const method: string = request.method;

    console.debug("url = ", url);
    console.debug("method = ", method);

    if(url.match(/\/api\/token/) && method === "POST") {
      return new Observable(resp => {
        resp.next(new HttpResponse({
          status: 200,
          body: DevToken
        }))
      });
    }

    if(url.match(/\/api\/v1\/user\/all/) && method === "GET") {
      return new Observable(resp => {
        resp.next(new HttpResponse({
          status: 200,
          body: FakeUserPagedData
        }))
      });
    }

    return next.handle(request); // fallback in case url isn't caught
  }
}

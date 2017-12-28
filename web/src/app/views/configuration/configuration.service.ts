import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";

@Injectable()
export class ConfigurationService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get configuration
   * @returns {Observable<any>}
   */
  get(): Observable<any> {
    return this.httpClient.get<any>('/api/v1/configuration');
  }

  /**
   * Save the configuration
   * @param configurationDto
   * @returns {Observable<any>}
   */
  post(configurationDto: any): Observable<any> {
    return this.httpClient.post('/api/v1/configuration', configurationDto);
  }
}

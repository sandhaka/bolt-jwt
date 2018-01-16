import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {HttpClient} from "@angular/common/http";
import {Authorization, AuthorizationDefinition} from "./model/authorizations";


@Injectable()
export class AuthorizationManagerService {

  constructor(private httpClient: HttpClient) {}

  /**
   * Get user authorizations list
   * @param {number} id
   * @returns {Observable<any>}
   */
  getUserAuthorizations(id: number): Observable<any> {
    return this.httpClient.get<Authorization[]>(`/api/v1/user/authorizations?id=${id}`);
  }

  /**
   * Get role authorizations list
   * @param {number} id
   * @returns {Observable<any>}
   */
  getRoleAuthorizations(id: number): Observable<any> {
    return this.httpClient.get<Authorization[]>(`/api/v1/role/authorizations?id=${id}`);
  }

  /**
   * Get all authorizations
   * @returns {Observable<any>}
   */
  getAuthorizationsDefinition(): Observable<any> {
    return this.httpClient.get<AuthorizationDefinition[]>('/api/v1/authorization');
  }

  /**
   * Assign an authorization to a user
   * @param command
   * @returns {Observable<any>}
   */
  addUserAuthorizations(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/user/add.auth', command);
  }

  /**
   * Remove an authorization from a user
   * @param command
   * @returns {Observable<any>}
   */
  removeUserAuthorization(command: any): Observable<any> {

    // Encode to a string to send via uri
    const authReduced = command.authorizations.reduce((a,b) => {
      return a+','+b;
    });

    return this.httpClient.delete(`/api/v1/user/rm.auth?userId=${command.userId}&authorizations=${authReduced}`);
  }

  /**
   * Assign an authorization to a role
   * @param command
   * @returns {Observable<any>}
   */
  addRoleAuthorizations(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/role/add.auth', command);
  }

  /**
   * Remove an authorization from a role
   * @param command
   * @returns {Observable<any>}
   */
  removeRoleAuthorization(command: any): Observable<any> {

    // Encode to a string to send via uri
    const authReduced = command.authorizations.reduce((a,b) => {
      return a+','+b;
    });

    return this.httpClient.delete(`/api/v1/role/rm.auth?roleId=${command.userId}&authorizations=${authReduced}`);
  }
}

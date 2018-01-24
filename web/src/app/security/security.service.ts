import {Injectable} from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import {HttpClient, HttpErrorResponse } from '@angular/common/http';
import {TokenResponse} from './token.response';
import {UtilityService} from "../shared/utils.service";
import {AuthenticationStatus} from "./auth-status";

@Injectable()
export class SecurityService {
  public token: string;

  constructor(private http: HttpClient) {
    // set token if saved in local storage
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.token = currentUser && currentUser.token;
  }

  /**
   * Retrieve a new json web token
   * @param {string} username
   * @param {string} password
   * @returns {Observable<boolean>}
   */
  getToken(username: string, password: string): Observable<boolean> {

    const body = { username: username, password: password };

    return this.http.post<TokenResponse>('/api/token', body)
      .map(response => {
        // login successful if there's a jwt token in the response
        const token = response.access_token;
        if (token) {
          this.storeToken(token);
          // return true to indicate successful login
          return true;
        } else {
          // return false to indicate failed login
          return false;
        }
      });
  }

  /**
   * Delete the current token from the local storage
   */
  logout(): void {
    // clear token remove user from local storage to log user out
    this.token = null;
    localStorage.removeItem('currentUser');
  }

  /**
   * Establish if the token is valid, if it's going to expire, renew it
   * @returns {AuthenticationStatus}
   */
  tokenCheck(): AuthenticationStatus {

    const status = {
      Authenticated: false,
      AdminUser: false,
      RootUser: false
    };

    // Retrieve the local copy of the token
    const storedToken = localStorage.getItem('currentUser');

    if (this.token && storedToken !== null) {

      const tokenData = JSON.parse(storedToken);

      if (tokenData.exp < Date.now()) {

        console.debug('Token expired');

        localStorage.removeItem('currentUser');

        return status;

      } else {

        status.Authenticated = true;

        const decodedUserToken = UtilityService.decodeToken(this.token);

        // Verify root user
        if(decodedUserToken.isRoot === "true") {
          status.RootUser = true;
          status.AdminUser = true;
        }

        // Verify the 'admin' authorization
        if(decodedUserToken.authorizations.includes("administrative")) {
          status.AdminUser = true;
        }
      }

      if(status.Authenticated) {

        // If the token is going to expire in less of a day renew it
        if (tokenData.exp > Date.now() && tokenData.exp < (Date.now() + 86400)) {

          this.tokenRenew().subscribe(
            (result) => {
              if (!result) {
                console.warn('Error on token renew');
              }
              console.debug('Token renewed');
            },
            (error: HttpErrorResponse) => {
              console.warn(`Error on token renew: ${error.message}`);
            });
        }
      }
    }

    return status;
  }

  /**
   * Renew the current token
   * @returns {Observable<boolean>}
   */
  private tokenRenew(): Observable<boolean> {

    return this.http.get<TokenResponse>('/api/tokenrenew')
      .map(response => {

        const token = response.access_token;

        if (token) {
          this.storeToken(token);
          return true;
        }

        return false;
      });
  }

  private storeToken(token: string) {
    this.token = token;

    const tokenData = UtilityService.decodeToken(this.token);

    // store username and jwt token in local storage to keep user logged in between page refreshes
    const userData = JSON.stringify(
      {
        username: tokenData.username,
        userId: tokenData.userId,
        token: token,
        exp: Date.now() + tokenData.exp
      });

    localStorage.setItem('currentUser', userData);
  }
}

import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";

@Injectable()
export class AccountService {

  constructor(private httpClient: HttpClient) { }

  activate(activateCommand: any): Observable<any> {
    return this.httpClient.post('/api/v1/account', activateCommand);
  }
}

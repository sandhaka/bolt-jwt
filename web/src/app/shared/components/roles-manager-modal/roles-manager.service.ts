import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {Role} from "./model/role";
import {AppEntity} from "../../common";

@Injectable()
export class RolesManagerService {

  constructor(private httpClient: HttpClient) { }

  getRoles(): Observable<Role[]> {
    return this.httpClient.get<Role[]>('/api/v1/role')
  }

  getAssignedRoles(entity: AppEntity): Observable<Role[]> {
    if(entity === AppEntity.User) {
      return this.httpClient.get<Role[]>('/api/v1/user/roles');
    } else if(entity === AppEntity.Group) {
      return this.httpClient.get<Role[]>('/api/v1/group/roles');
    }
  }
}

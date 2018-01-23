import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {Role} from "./model/role";
import {AppEntity} from "../../common";
import {AssignedRole} from "./model/assignedRole";

@Injectable()
export class RolesManagerService {

  constructor(private httpClient: HttpClient) { }

  getRoles(): Observable<Role[]> {
    return this.httpClient.get<Role[]>('/api/v1/role/all')
  }

  getAssignedRoles(entity: AppEntity, entityId: number): Observable<AssignedRole[]> {
    if(entity === AppEntity.User) {
      return this.httpClient.get<AssignedRole[]>(`/api/v1/user/roles?id=${entityId}`);
    } else if(entity === AppEntity.Group) {
      return this.httpClient.get<AssignedRole[]>(`/api/v1/group/roles?id=${entityId}`);
    }
  }

  assignRoles(entity: AppEntity, command: any): Observable<any> {
    if(entity === AppEntity.User) {
      return this.httpClient.post('/api/v1/user/edit.roles', command);
    } else if(entity === AppEntity.Group) {
      return this.httpClient.post('/api/v1/group/edit.roles', command);
    }
  }
}




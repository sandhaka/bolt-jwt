import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AssignedGroup} from "./model/assignedGroup";
import {Group} from "./model/group";
import {Observable} from "rxjs";

@Injectable()
export class GroupsManagerModalService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get the list of assigned groups
   * @param {number} id
   * @returns {Observable<any>}
   */
  getAssignedGroups(id: number): Observable<any> {
    return this.httpClient.get<AssignedGroup>(`/api/v1/user/groups?id=${id}`);
  }

  /**
   * Retrieve all groups
   * @returns {Observable<any>}
   */
  getGroups(): Observable<any> {
    return this.httpClient.get<Group[]>('/api/v1/group/all');
  }

  /**
   * Edit user groups
   * @param command
   * @returns {Observable<any>}
   */
  assignGroups(command: any): Observable<any> {
    return this.httpClient.post('/api/v1/user/edit.groups', command);
  }
}

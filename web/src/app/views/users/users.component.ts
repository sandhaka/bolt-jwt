import {Component, OnInit} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {UsersService} from "./users.service";

@Component({
  templateUrl: 'users.component.html'
})
export class UsersComponent implements OnInit {

  columnNames = [
    {name: 'Name'},
    {name: 'Surname'},
    {name: 'Username'},
    {name: 'Email'}
  ];

  /**
   * Service callback ref to retrieve users from server
   */
  getDataFunc: Function;

  constructor(private usersService: UsersService) { }

  ngOnInit(): void {
    // Bind
    this.getDataFunc = this.getData.bind(this);
  }

  /**
   * The service callback
   * @param params
   * @returns {Observable<PagedData<any>>}
   */
  getData(params: any): Observable<PagedData<any>> {
    return this.usersService.getUsersPaged(params);
  }
}

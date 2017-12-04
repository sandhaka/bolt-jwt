import {Component, OnInit} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {UsersService} from "./users.service";
import {UserDto} from "./UserDto";
import {DataTableService} from "../../shared/datatable/data-table.service";
import {Page} from "../../shared/datatable/model/page";

@Component({
  templateUrl: 'users.component.html',
  styleUrls: ['users.component.scss'],
  providers: [DataTableService]
})
export class UsersComponent implements OnInit {

  columnNames = [
    {name: 'Name'},
    {name: 'Surname'},
    {name: 'Username'},
    {name: 'Email'}
  ];

  filtersAccordionOpened = false;

  /**
   * Service callbacks ref
   */
  getDataFunc: Function;
  onSelectFunc: Function;

  /**
   * Keep a copy of the item to working on
   */
  selectedUser: UserDto;

  constructor(
    private usersService: UsersService,
    private dataTableService: DataTableService) {
  }

  ngOnInit(): void {
    // Bind
    this.getDataFunc = this.getData.bind(this);
    this.onSelectFunc = this.onSelect.bind(this);
  }

  /**
   * The service callback
   * @param params
   * @returns {Observable<PagedData<any>>}
   */
  getData(params: Page): Observable<PagedData<any>> {
    this.selectedUser = null;
    return this.usersService.getUsersPaged(params);
  }

  /**
   * Copy the selected user into dto
   * @param {any} selected
   */
  onSelect({selected}) {
    const selectedUser = selected[0];
    this.selectedUser = new UserDto();
    this.selectedUser.Id = selectedUser.id;
    this.selectedUser.Username = selectedUser.username;
    this.selectedUser.Name = selectedUser.name;
    this.selectedUser.Surname = selectedUser.surname;
    this.selectedUser.Email = selectedUser.email;
  }

  /**
   * Update the table when the user edit a record
   * @param editedInfo
   */
  onUserEdit(editedInfo: any) {
    const editedRow = {
      id: editedInfo.id,
      name: editedInfo.command.name,
      surname: editedInfo.command.surname,
      username: editedInfo.command.userName,
      email: this.selectedUser.Email
    };
    this.dataTableService.editRow(editedRow);
  }

  /**
   * Open/Close filters accordion
   */
  toggleAccordion() {
    this.filtersAccordionOpened = !this.filtersAccordionOpened
  }
}

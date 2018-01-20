import {Component, OnInit} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {UsersService} from "./users.service";
import {UserDto} from "./UserDto";
import {DataTableService} from "../../shared/datatable/data-table.service";
import {Page} from "../../shared/datatable/model/page";
import {AppEntity} from "../../shared/common";
import {BsModalService} from "ngx-bootstrap";
import {CreateUserModalComponent} from "./create-user-modal/create-user-modal.component";
import {GenericModalComponent} from "../../shared/modals";
import {RolesManagerModalComponent} from "../../shared/components/roles-manager-modal/roles-manager-modal.component";

@Component({
  templateUrl: 'users.component.html',
  styleUrls: ['users.component.scss'],
  // Provide the data table service for this component and his children
  providers: [DataTableService]
})
export class UsersComponent implements OnInit {

  columnNames = [
    {name: 'Name'},
    {name: 'Surname'},
    {name: 'Username'},
    {name: 'Email'}
  ];

  /**
   * Filters object for data table component
   * @type {any[]}
   */
  filters = [
    {label:'Name', type: 'string', name: 'name', cssClass: 'col-sm-3'},
    {label:'Surname', type: 'string', name: 'surname', cssClass: 'col-sm-3'},
    {label:'Username', type: 'string', name: 'username', cssClass: 'col-sm-3'},
    {label:'Email', type: 'string', name: 'email', cssClass: 'col-sm-3'}
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
  selectedUser: UserDto = null;

  /**
   * Type entity
   */
  userEntity: AppEntity = AppEntity.User;

  constructor(
    private usersService: UsersService,
    private modalService: BsModalService,
    private dataTableService: DataTableService) {  }

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
   * User selection handler
   * Copy the selected user into a local dto
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
   * Update the table after a edit
   */
  onUserEdit() {
    this.dataTableService.invokeRowEdit();
  }

  /**
   * Update the table after a user deletion
   */
  onUserDelete() {
    this.dataTableService.invokeRowDelete();
  }

  /**
   * Open/Close filters accordion
   */
  toggleAccordion() {
    this.filtersAccordionOpened = !this.filtersAccordionOpened
  }

  /**
   * Open a modal to add a new user
   */
  addNew() {
    const bsUserCreationModal = this.modalService.show(CreateUserModalComponent);
    bsUserCreationModal.content.modalTitle = "New user";
    bsUserCreationModal.content.modalCss = "modal-info";
    bsUserCreationModal.content.onCreated.subscribe(
      (email) => {

        bsUserCreationModal.hide();

        // Success modal
        const bsUserCreatedModal = this.modalService.show(GenericModalComponent);
        bsUserCreatedModal.content.modalTitle = "Created successfully";
        bsUserCreatedModal.content.modalClass = "modal-success";
        bsUserCreatedModal.content.modalText = `The new account needs to be activate. An email has been sent to ${email}`;
      }
    );
  }

  openRolesManager() {
    const bsRolesManagerModal = this.modalService.show(RolesManagerModalComponent);
    bsRolesManagerModal.content.modalTitle = "User roles manager";
    bsRolesManagerModal.content.modalCss = "modal-info";

    // Manage roles of user entity
    bsRolesManagerModal.content.serviceEntity = AppEntity.User;

    // Load modal content
    //bsRolesManagerModal.content.load();
  }
}

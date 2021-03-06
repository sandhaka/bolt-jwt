import {Component, OnInit} from "@angular/core";
import {AppEntity} from "../../shared/common";
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {DataTableService} from "../../shared/datatable/data-table.service";
import {RoleDto} from "./roleDto";
import {RolesService} from "./roles.service";
import {BsModalService} from "ngx-bootstrap";
import {CreateRoleModalComponent} from "./create-role-modal/create-role-modal.component";
import {ViewRoleUsageModalComponent} from "./view-role-usage-modal/view-role-usage-modal.component";

@Component({
  templateUrl: 'roles.component.html',
  styleUrls: ['roles.component.scss'],
  // Provide the data table service for this component and his children
  providers: [DataTableService]
})
export class RolesComponent implements OnInit {

  columnNames = [
    {name: 'Description'}
  ];

  selectedRole: RoleDto = null;

  /**
   * Filters object for data table component
   * @type {any[]}
   */
  filters = [
    {label:'Description', type: 'string', name: 'description', cssClass: 'col-sm-12'}
  ];

  filtersAccordionOpened = false;

  /**
   * Service callbacks ref
   */
  getDataFunc: Function;
  onSelectFunc: Function;

  /**
   * Type entity
   */
  entity: AppEntity = AppEntity.Role;

  constructor(private dataTableService: DataTableService,
              private rolesService: RolesService,
              private modalService: BsModalService) {}

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
    this.selectedRole = null;
    return this.rolesService.getRolesPaged(params);
  }

  /**
   * Role selection handler
   * Copy the selected role into a local dto
   * @param {any} selected
   */
  onSelect({selected}) {
    const selectedRole = selected[0];
    this.selectedRole = new RoleDto();
    this.selectedRole.Description = selectedRole.description;
    this.selectedRole.Id = selectedRole.id;
  }

  /**
   * Refresh the table
   */
  onUpdate() {
    this.dataTableService.invokeReload();
  }

  /**
   * Open a modal to add a new role
   */
  addNew() {
    const bsRoleCreationModal = this.modalService.show(CreateRoleModalComponent);
    bsRoleCreationModal.content.modalTitle = "New role";
    bsRoleCreationModal.content.modalCss = "modal-info";
    bsRoleCreationModal.content.onCreated.subscribe(
      () => {
        bsRoleCreationModal.hide();
        this.dataTableService.invokeReload();
      }
    )
  }

  /**
   * Open/Close filters accordion
   */
  toggleAccordion() {
    this.filtersAccordionOpened = !this.filtersAccordionOpened
  }

  /**
   * Show role usage
   */
  viewUsage() {
    const bsRoleUsageModal = this.modalService.show(ViewRoleUsageModalComponent);
    bsRoleUsageModal.content.roleId = this.selectedRole.Id;
    bsRoleUsageModal.content.modalTitle = "Role usage";
    bsRoleUsageModal.content.modalCss = "modal-info";
    bsRoleUsageModal.content.load();
  }
}

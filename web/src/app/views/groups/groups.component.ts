import {Component, OnInit} from "@angular/core";
import {DataTableService} from "../../shared/datatable/data-table.service";
import {AppEntity} from "../../shared/common";
import {BsModalService} from "ngx-bootstrap";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs/Observable";
import {GroupDto} from "./groupDto";
import {GroupsService} from "./groups.service";

@Component({
  templateUrl: './groups.component.html',
  styleUrls: ['/groups.component.scss'],
  // Provide the data table service for this component and his children
  providers: [DataTableService]
})
export class GroupsComponent implements OnInit {

  columnNames = [
    {name: 'Description'}
  ];

  selectedGroup: GroupDto;

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
  entity: AppEntity = AppEntity.Group;

  constructor(private dataTableService: DataTableService,
              private groupsService: GroupsService,
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
    this.selectedGroup = null;
    return this.groupsService.getGroupsPaged(params);
  }

  /**
   * Group selection handler
   * Copy the selected group into a local dto
   * @param {any} selected
   */
  onSelect({selected}) {
    const selectedGroup = selected[0];
    this.selectedGroup = new GroupDto();
    this.selectedGroup.Description = selectedGroup.description;
    this.selectedGroup.Id = selectedGroup.id;
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
    // const bsRoleCreationModal = this.modalService.show();
    // bsRoleCreationModal.content.modalTitle = "New group";
    // bsRoleCreationModal.content.modalCss = "modal-info";
    // bsRoleCreationModal.content.onCreated.subscribe(
    //   () => {
    //     bsRoleCreationModal.hide();
    //     this.dataTableService.invokeReload();
    //   }
    // );
  }

  /**
   * Open/Close filters accordion
   */
  toggleAccordion() {
    this.filtersAccordionOpened = !this.filtersAccordionOpened
  }
}

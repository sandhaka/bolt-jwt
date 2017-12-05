import {Component, Input} from "@angular/core";
import {ReactiveFormComponent} from "../../base/reactiveForm.component";
import {DataTableService} from "../data-table.service";
import {FormGroup} from "@angular/forms";

@Component({
  selector: 'app-data-table-filters',
  templateUrl: 'data-table-filters.component.html'
})
export class DataTableFiltersComponent extends ReactiveFormComponent {

  /**
   * Filters collection
   *
   * { label, type, name, cssClass }
   *
   * label: Input box label
   *
   * type: Input type: 'string', 'boolean', 'datetime', 'select'
   * TODO: At the moment, only 'string' is implemented
   *
   * name: Element name
   *
   * cssClass: CSS class used to render the element field
   *
   * Example:
   * [{label:'Name', type: 'string', name: 'name', cssClass: 'col-sm-3'}]
   *
   */
  @Input('filters') filters: any[];

  constructor(
    private dataTableService: DataTableService) {
    super();

    this.form = new FormGroup({});
  }

  /**
   * Parsing fields, build a filters object and publish it
   */
  applyFilters() {
    const values = this.form.value;

    const filters = [];

    // Collects the filters with a operand
    for(const f of this.filters) {
      if(values[f.name]) {
        filters.push({
          name: f.name,
          operand: this.getFilterOperand(f.type),
          value: values[f.name]
        });
      }
    }

    // Notify filters to apply
    this.dataTableService.invokeApplyFilters(filters);

    // Filters are empty then reset the form dirty status
    if(filters.length === 0) {
      this.form.markAsPristine();
    }
  }

  /**
   * Reset filters fields and apply
   */
  reset() {
    this.form.reset();
    this.applyFilters();
  }

  /**
   * Return a sql operand
   * @param {string} type
   * @returns {string}
   */
  private getFilterOperand(type: string): string {
    switch (type) {
      case 'string': {
        return 'like';
      }
      default: {
        return '=';
      }
    }
  }
}

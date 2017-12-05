import {Component, Input, OnInit} from "@angular/core";
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-data-table-filters-string',
  template: `
    <div class="form-group" [formGroup]="form">
      <label class="control-label">{{filter.label}}</label>
      <div class="input-group">
        <input class="form-control" type="text" [formControlName]="filter.name">
      </div>
    </div>
  `
})
export class DataTableFiltersStringComponent implements OnInit {

  @Input('filter') filter: any;

  @Input('form') form: FormGroup;

  constructor() { }

  ngOnInit(): void {
    this.form.addControl(this.filter.name, new FormControl());
  }
}

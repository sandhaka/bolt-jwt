import {Component, EventEmitter, Input, OnChanges, Output, SimpleChanges} from "@angular/core";
import {GroupDto} from "../groupDto";
import {ReactiveFormComponent} from "../../../shared/base/reactiveForm.component";
import {HttpErrorResponse} from "@angular/common/http";
import {FormBuilder, Validators} from "@angular/forms";
import {UtilityService} from "../../../shared/utils.service";
import {GroupsService} from "../groups.service";

@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html'
})
export class GroupDetailsComponent extends ReactiveFormComponent implements OnChanges  {

  @Input('group') group: GroupDto;

  @Output('changed') changed: EventEmitter<any> = new EventEmitter();

  formErrors = {
    description: ''
  };

  isPending = false;

  constructor(
    private formBuilder: FormBuilder,
    private utils: UtilityService,
    private groupService: GroupsService
  ) {
    super();

    this.validationMessages = {
      'description': {
        'required': 'Required'
      }
    };
    this.form = this.formBuilder.group({
      'description': [
        '',
        Validators.required
      ]
    });

    // Trigger validation on form data change
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });

    this.onDataChanged();
  }

  saveGroupDetails() {

    this.isPending = true;

    const command = {
      Id: this.group.Id,
      Description: this.form.value.description
    };

    this.groupService.edit(command).subscribe(
      response => {
        this.isPending = false;
        this.changed.emit();
      },
      (errorResponse: HttpErrorResponse) => {
        this.utils.handleHttpError(errorResponse);
        this.isPending = false;
      }
    );
  }

  delete() {
    const deleteGroupCallback = function() {

      this.groupService.delete(this.group.Id).subscribe(
        () => {
          this.changed.emit();
        },
        (errorResponse: HttpErrorResponse) => this.utils.handleHttpError(errorResponse)
      );
    }
      .bind(this);

    // Call the delete service after confirmation
    this.utils.openConfirmModal(
      "Delete confirmation",
      `Are you sure to delete the group: ${this.group.Description}?`, "modal-warning",
      deleteGroupCallback
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.resetForm();
  }

  private resetForm() {
    this.form.reset({
      description: this.group.Description
    });
  }
}

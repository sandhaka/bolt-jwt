import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {ReactiveFormComponent} from "../../../shared/base/reactiveForm.component";
import {UserDto} from "../UserDto";
import {UsersService} from "../users.service";
import {HttpErrorResponse} from "@angular/common/http";
import {BsModalService} from "ngx-bootstrap";
import {UtilityService} from "../../../shared/utils.service";

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss']
})
export class UserDetailsComponent extends ReactiveFormComponent implements OnInit, OnChanges {

  /**
   * User
   */
  @Input('user') user: UserDto;

  /**
   * Edited user data
   * @type {EventEmitter<any>}
   */
  @Output('editedUserInfo') editedUserInfo: EventEmitter<any> = new EventEmitter();

  /**
   * Deleted used
   * @type {EventEmitter<number>} User id
   */
  @Output('deletedUser') deletedUser: EventEmitter<number> = new EventEmitter();

  /**
   * Form model
   */
  userFormModel: UserDto;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UsersService,
    private modalService: BsModalService,
    private utils: UtilityService
  ) {

    super();

    this.formErrors = {
      'name': '',
      'surname': '',
      'username': ''
    };

    this.validationMessages = {
      'name': {
        'required': 'Required'
      },
      'surname': {
        'required': 'Required'
      },
      'username': {
        'required': 'Required'
      }
    };

    this.userFormModel = new UserDto();

    this.form = this.formBuilder.group({
      'name': [
        this.userFormModel.Name,
        Validators.required
      ],
      'surname': [
        this.userFormModel.Surname,
        Validators.required
      ],
      'username': [
        this.userFormModel.Username,
        Validators.required
      ]
    });

    // Trigger validation on form data change
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });

    this.onDataChanged();
  }

  /**
   * Save user details
   */
  saveUserDetails() {

    const formValue = this.form.value;

    const editCommand = {
      Id: this.user.Id,
      Name: formValue.name,
      Surname: formValue.surname,
      Username: formValue.username
    };

    this.userService.edit(editCommand).subscribe(
      response => {
        this.editedUserInfo.emit(response.result);
    },
      (errorResponse: HttpErrorResponse) => this.utils.handleHttpError(errorResponse));
  }

  /**
   * Delete the selected user
   */
  delete() {

    const deleteUserCallback = function() {

      this.userService.delete(this.user.Id).subscribe(
        () => {
          this.deletedUser.emit();
        },
        (errorResponse: HttpErrorResponse) => this.utils.handleHttpError(errorResponse)
      );
    }
    .bind(this);

    // Call the delete service after confirmation
    this.utils.openConfirmModal(
      "Delete confirmation",
      `Are you sure to delete the user with id: ${this.user.Id}?`, "modal-warning",
      deleteUserCallback
    );
  }

  ngOnInit() {}

  /**
   * Update the form values on parent component selection changed
   * @param {SimpleChanges} changes
   */
  ngOnChanges(changes: SimpleChanges): void {
    this.resetForm();
  }

  private resetForm() {
    this.form.reset({
      surname: this.user.Surname,
      name: this.user.Name,
      username: this.user.Username
    });
  }
}

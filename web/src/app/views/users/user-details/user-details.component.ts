import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {ReactiveFormComponent} from "../../../shared/base/reactiveForm.component";
import {UserDto} from "../UserDto";
import {UsersService} from "../users.service";
import {HttpErrorResponse} from "@angular/common/http";
import {GenericModalComponent, ConfirmModalComponent} from '../../../shared/modals';
import {BsModalRef, BsModalService} from "ngx-bootstrap";

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

  private bsModalRef: BsModalRef;
  private bsConfirmModalRef: BsModalRef;

  /**
   * Form model
   */
  userFormModel: UserDto;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UsersService,
    private modalService: BsModalService
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

  onNgSubmit() {

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
      (errorResponse: HttpErrorResponse) => this.handleHttpError(errorResponse));
  }

  /**
   * Delete the selected user
   */
  delete() {

    const deleteUserCallback = function() {

      const deleteCommand = {
        Id: this.user.Id
      };

      this.userService.delete(deleteCommand).subscribe(
        () => {
          this.deletedUser.emit();
        },
        (errorResponse: HttpErrorResponse) => this.handleHttpError(errorResponse)
      );
    }
    .bind(this);

    // Call the delete service after confirmation
    this.openConfirmModal(
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

  private openModal(title: string, body: string, cssClass: string) {
    this.bsModalRef = this.modalService.show(GenericModalComponent);
    this.bsModalRef.content.modalTitle = title;
    this.bsModalRef.content.modalClass = cssClass;
    this.bsModalRef.content.modalText = body;
  }

  private handleHttpError(errorResponse: HttpErrorResponse) {
    const errorDetails = errorResponse.error && errorResponse.error.Message ?
      errorResponse.error.Message :
      errorResponse.message;
    this.openModal('Error', `${errorResponse.statusText}: ${errorDetails}`, 'modal-danger');
  }

  private openConfirmModal(title: string, body: string, cssClass: string, callback: () => void) {
    this.bsConfirmModalRef = this.modalService.show(ConfirmModalComponent);
    this.bsConfirmModalRef.content.modalTitle = title;
    this.bsConfirmModalRef.content.modalClass = cssClass;
    this.bsConfirmModalRef.content.modalText = body;
    this.bsConfirmModalRef.content.onClose.subscribe(result => {
      if(result === true) {
        callback();
      }
    });
  }

  private resetForm() {
    this.form.reset({
      surname: this.user.Surname,
      name: this.user.Name,
      username: this.user.Username
    });
  }
}

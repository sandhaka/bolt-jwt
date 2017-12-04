import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {ReactiveFormComponent} from "../../../shared/base/reactiveForm.component";
import {UserDto} from "../UserDto";
import {UsersService} from "../users.service";
import {HttpErrorResponse} from "@angular/common/http";
import {ModalComponent} from "../../../shared/modals/modal.component";
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

  private bsModalRef: BsModalRef;

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

    this.userService.saveUserDetails(editCommand).subscribe(
      response => {
        this.editedUserInfo.emit(response.result);
    },
      (errorResponse: HttpErrorResponse) => {
        const errorDetails = errorResponse.error && errorResponse.error.Message ?
          errorResponse.error.Message :
          errorResponse.message;
        this.openModal('Error', `${errorResponse.statusText}: ${errorDetails}`, 'modal-danger');
    });
  }

  ngOnInit() {

  }

  /**
   * Update the form values on parent component selection changed
   * @param {SimpleChanges} changes
   */
  ngOnChanges(changes: SimpleChanges): void {
    this.resetForm();
  }

  /**
   * Open a modal dialog
   * @param {string} title
   * @param {string} body
   * @param {string} cssClass
   */
  private openModal(title: string, body: string, cssClass: string) {
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.modalTitle = title;
    this.bsModalRef.content.modalClass = cssClass;
    this.bsModalRef.content.modalText = body;
  }

  private resetForm() {
    this.form.reset({
      surname: this.user.Surname,
      name: this.user.Name,
      username: this.user.Username
    });
  }
}

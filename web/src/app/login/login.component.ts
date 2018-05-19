import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {SecurityService} from '../security/security.service';
import {Router} from '@angular/router';
import {HttpErrorResponse} from "@angular/common/http";
import {BsModalRef, BsModalService} from "ngx-bootstrap";
import {GenericModalComponent} from "../shared/modals";
import {ReactiveFormComponent} from "../shared/base/reactiveForm.component";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent extends ReactiveFormComponent implements OnInit {

  private formBuilder: FormBuilder;
  private authService: SecurityService;
  private router: Router;
  private userDto: UserDto;
  private bsModalRef: BsModalRef;
  private bsModalService: BsModalService;
  form: FormGroup;

  formErrors = {
    username: '',
    password: ''
  };

  loginError = '';

  constructor(authService: SecurityService, formBuilder: FormBuilder, router: Router, modalService: BsModalService) {

    super();

    this.validationMessages = {
      'username': {
        'required': 'Required'
      },
      'password': {
        'required': 'Required'
      }
    };

    this.formBuilder = formBuilder;
    this.authService = authService;
    this.router = router;
    this.userDto = new UserDto();
    this.bsModalService = modalService;

    this.form = this.formBuilder.group({
      'username': [
        this.userDto.username,
        Validators.required
      ],
      'password': [
        this.userDto.password,
        Validators.required
      ]
    });

    /**
     * Trigger validation on form data change
     */
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });
    this.onDataChanged();
  }

  /**
   * forward to dashboard page if the user is authenticated
   */
  ngOnInit() {
    if (this.authService.tokenCheck()) {
      this.router.navigate(['/dashboard']);
      return;
    }
  }

  /**
   * Login
   */
  onNgSubmit() {
    if (this.form.invalid) {
      return;
    }
    this.userDto = this.form.value;
    this.authService.getToken(this.userDto.username, this.userDto.password).subscribe(
      (result: boolean) => {
        if (result) {
          this.router.navigate(['/dashboard']);
        } else {
          this.loginError = 'Username/password not valid';
        }
      },
      (error: HttpErrorResponse) => {
        if(error.status === 400) {
          this.loginError = 'Username/password not valid';
        } else {
          this.openModal('Error', `${error.statusText}: ${error.message}`, 'modal-danger');
        }
      }
    );
  }

  /**
   * Open a modal dialog
   * @param {string} title
   * @param {string} body
   * @param {string} cssClass
   */
  private openModal(title: string, body: string, cssClass: string) {
    this.bsModalRef = this.bsModalService.show(GenericModalComponent);
    this.bsModalRef.content.modalTitle = title;
    this.bsModalRef.content.modalClass = cssClass;
    this.bsModalRef.content.modalText = body;
  }
}

class UserDto {
  username: string;
  password: string;
}

import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthenticationService} from '../security/authentication.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

  private formBuilder: FormBuilder;
  private authService: AuthenticationService;
  private router: Router;
  private userDto: UserDto;
  form: FormGroup;

  loginError = '';
  formErrors = {
    'username': '',
    'password': ''
  };

  validationMessages = {
    'username': {
      'required': 'Required'
    },
    'password': {
      'required': 'Required'
    }
  };

  constructor(authService: AuthenticationService, formBuilder: FormBuilder, router: Router) {

    this.formBuilder = formBuilder;
    this.authService = authService;
    this.router = router;
    this.userDto = new UserDto();

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

    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });
    this.onDataChanged();
  }

  ngOnInit() {
    if (this.authService.tokenCheck()) {
      this.router.navigate(['/dashboard']);
      return;
    }
  }

  /**
   * Login action
   */
  onNgSubmit() {
    this.userDto = this.form.value;
    this.authService.getToken(this.userDto.username, this.userDto.password).subscribe(
      (result: any) => {
        if (result) {
          this.router.navigate(['/dashboard']);
        } else {
          this.loginError = 'Username/password not valid';
        }
      },
      (error: any) => {
        this.loginError = 'Username/password not valid';
      }
    );
  }

  /**
   * Setup the error messages
   * @param data
   */
  private onDataChanged(data?: any) {
    if (!this.form) {
      return;
    }

    const _form = this.form;

    for (const field in this.formErrors) {
      if (this.formErrors.hasOwnProperty(field)) {
        const control = _form.get(field);
        this.formErrors[field] = '';

        if (control && control.dirty && !control.valid) {
          const messages = this.validationMessages[field];
          for (const key in control.errors) {
            if (control.errors.hasOwnProperty(key)) {
              this.formErrors[field] += messages[key] + ' ';
            }
          }
        }
      }
    }
  }
}

class UserDto {
  username: string;
  password: string;
}

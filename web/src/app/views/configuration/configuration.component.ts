import { Component, OnInit } from '@angular/core';
import {ReactiveFormComponent} from "../../shared/base/reactiveForm.component";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ConfigurationService} from "./configuration.service";
import {HttpErrorResponse} from "@angular/common/http";
import {UtilityService} from "../../shared/utils.service";
import {ANIMATION_TYPES} from "ngx-loading";

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss']
})
export class ConfigurationComponent extends ReactiveFormComponent implements OnInit {

  isLoading = false;

  // loading splash screen configuration
  loadingConfig = {
    backdropBorderRadius: '14px',
    animationType: ANIMATION_TYPES.wanderingCubes
  };

  constructor(
    private formBuilder: FormBuilder,
    private configurationService: ConfigurationService,
    private utils: UtilityService
  ) {
    super();

    // Form setup
    this.formErrors = {
      'hostname': '',
      'port': '',
      'username': '',
      'password': '',
      'email': '',
      'endpointfqdn': '',
      'endpointPort': '',
      'rootPassword': '',
      'rootPasswordConfirmation': ''
    };

    this.validationMessages = {
      'hostname': {
        'required': 'Required'
      },
      'port': {
        'required': 'Required'
      },
      'username': {
        'required': 'Required'
      },
      'password': {
        'required': 'Required'
      },
      'email': {
        'required': 'Required',
        'email': 'Must be a valid email'
      },
      'endpointfqdn': {
        'required': 'Required'
      },
      'endpointPort': {
        'required': 'Required'
      },
      'rootPassword': {
        'required': 'Required',
        'minlength': 'Minimum 6 character length'
      },
      'rootPasswordConfirmation': {
        'required': 'Required',
        'minlength': 'Minimum 6 character length'
      },
    };

    this.form = this.formBuilder.group({
      'hostname': ['', Validators.required],
      'port': ['', Validators.required],
      'username': ['', Validators.required],
      'password': ['', Validators.required],
      'email': ['',
        [
          Validators.required,
          Validators.email
        ]
      ],
      'endpointfqdn': ['', Validators.required],
      'endpointPort': ['', Validators.required],
      'rootPassword': ['',
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ],
      'rootPasswordConfirmation': ['',
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ]
    },{validator: this.matchingPassword});

    // Trigger validation on form data change
    this.form.valueChanges.subscribe((data) => {
      this.onDataChanged(data);
    });

    this.onDataChanged();
  }

  ngOnInit() {

    this.isLoading = true;

    // Load configuration at startup
    this.configurationService.get().subscribe(
      (data: any) => {

        this.form.setValue({
          hostname: data.config.smtpHostName,
          port: data.config.smtpPort,
          username: data.config.smtpUserName,
          password: data.config.smtpPassword,
          email: data.config.smtpEmail,
          endpointfqdn: data.config.endpointFqdn,
          endpointPort: data.config.endpointPort,
          rootPassword: data.config.rootPassword,
          rootPasswordConfirmation: data.config.rootPassword,
        });

        this.isLoading = false;
      },
      (error: HttpErrorResponse) => {
        this.utils.handleHttpError(error);
        this.isLoading = false;
      }
    );
  }

  /**
   * Save configuration
   */
  save() {

    this.isLoading = true;

    const formValue = this.form.value;

    const configDto = {
      SmtpHostname: formValue.hostname,
      SmtpPort: formValue.port,
      SmtpUserName: formValue.username,
      SmtpPassword: formValue.password,
      EndpointFqdn: formValue.endpointfqdn,
      EndpointPort: formValue.endpointPort,
      RootPassword: formValue.rootPassword,
      RootPasswordConfirmation: formValue.rootPasswordConfirmation
    };

    this.configurationService.post(configDto).subscribe(
      () => {
        this.isLoading = false;
      },
      (error: HttpErrorResponse) => {
        this.utils.handleHttpError(error);
        this.isLoading = false;
      }
    );
  }

  matchingPassword(group: FormGroup) {
    const pass = group.value;
    return (pass.rootPassword === pass.rootPasswordConfirmation) ? null : {
      invalid: true
    };
  }
}

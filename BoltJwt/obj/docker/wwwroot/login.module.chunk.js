webpackJsonp(["login.module"],{

/***/ "../../../../../src/app/login/login.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"app flex-row align-items-center\">\n  <div class=\"container\">\n    <div class=\"row justify-content-center\">\n      <div class=\"col-md-8\">\n        <div class=\"card-group\">\n          <div class=\"card p-4\">\n            <div class=\"card-body\">\n              <h1>Login</h1>\n              <form [formGroup]=\"form\" novalidate (ngSubmit)=\"onNgSubmit()\">\n                <div class=\"form-group\">\n                  <div class=\"input-group\">\n                    <span class=\"input-group-addon\"><i class=\"icon-user\"></i></span>\n                    <input type=\"text\" class=\"form-control\" placeholder=\"Username\" formControlName=\"username\">\n                  </div>\n                  <span style=\"color: red\" *ngIf=\"formErrors.username\">{{formErrors.username}}</span>\n                </div>\n                <div class=\"form-group\">\n                  <div class=\"input-group\">\n                    <span class=\"input-group-addon\"><i class=\"icon-lock\"></i></span>\n                    <input type=\"password\" class=\"form-control\" placeholder=\"Password\" formControlName=\"password\">\n                  </div>\n                  <span style=\"color: red\" *ngIf=\"formErrors.password\">{{formErrors.password}}</span>\n                </div>\n                <div class=\"row\">\n                  <div class=\"col-6\">\n                    <input [disabled]=\"form.invalid\" type=\"submit\" class=\"btn btn-primary px-4\" value=\"Login\"/>\n                  </div>\n                  <div class=\"col-6 text-right\">\n                    <button type=\"button\" class=\"btn btn-link px-0\">Forgot password?</button>\n                  </div>\n                  <div class=\"col-6\" *ngIf=\"loginError\">\n                    <span style=\"color: red\">{{loginError}}</span>\n                  </div>\n                </div>\n              </form>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"

/***/ }),

/***/ "../../../../../src/app/login/login.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LoginComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__security_authentication_service__ = __webpack_require__("../../../../../src/app/security/authentication.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_ngx_bootstrap__ = __webpack_require__("../../../../ngx-bootstrap/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_modals_modal_component__ = __webpack_require__("../../../../../src/app/shared/modals/modal.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var LoginComponent = /** @class */ (function () {
    function LoginComponent(authService, formBuilder, router, modalService) {
        var _this = this;
        this.loginError = '';
        this.formErrors = {
            'username': '',
            'password': ''
        };
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
                __WEBPACK_IMPORTED_MODULE_1__angular_forms__["f" /* Validators */].required
            ],
            'password': [
                this.userDto.password,
                __WEBPACK_IMPORTED_MODULE_1__angular_forms__["f" /* Validators */].required
            ]
        });
        /**
         * Trigger validation on form data change
         */
        this.form.valueChanges.subscribe(function (data) {
            _this.onDataChanged(data);
        });
        this.onDataChanged();
    }
    /**
     * forward to dashboard page if the user is authenticated
     */
    LoginComponent.prototype.ngOnInit = function () {
        if (this.authService.tokenCheck()) {
            this.router.navigate(['/dashboard']);
            return;
        }
    };
    /**
     * Login
     */
    LoginComponent.prototype.onNgSubmit = function () {
        var _this = this;
        if (this.form.invalid) {
            return;
        }
        this.userDto = this.form.value;
        this.authService.getToken(this.userDto.username, this.userDto.password).subscribe(function (result) {
            if (result) {
                _this.router.navigate(['/dashboard']);
            }
            else {
                _this.loginError = 'Username/password not valid';
            }
        }, function (error) {
            if (error.status === 400) {
                _this.loginError = 'Username/password not valid';
            }
            else {
                _this.openModal('Error', error.statusText + ": " + error.message, 'modal-danger');
            }
        });
    };
    /**
     * Open a modal dialog
     * @param {string} title
     * @param {string} body
     * @param {string} cssClass
     */
    LoginComponent.prototype.openModal = function (title, body, cssClass) {
        this.bsModalRef = this.bsModalService.show(__WEBPACK_IMPORTED_MODULE_5__shared_modals_modal_component__["a" /* ModalComponent */]);
        this.bsModalRef.content.modalTitle = title;
        this.bsModalRef.content.modalClass = cssClass;
        this.bsModalRef.content.modalText = body;
    };
    /**
     * Setup the error messages
     * @param data
     */
    LoginComponent.prototype.onDataChanged = function (data) {
        if (!this.form) {
            return;
        }
        var _form = this.form;
        for (var field in this.formErrors) {
            if (this.formErrors.hasOwnProperty(field)) {
                var control = _form.get(field);
                this.formErrors[field] = '';
                if (control && control.dirty && !control.valid) {
                    var messages = this.validationMessages[field];
                    for (var key in control.errors) {
                        if (control.errors.hasOwnProperty(key)) {
                            this.formErrors[field] += messages[key] + ' ';
                        }
                    }
                }
            }
        }
    };
    LoginComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-login',
            template: __webpack_require__("../../../../../src/app/login/login.component.html")
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__security_authentication_service__["a" /* AuthenticationService */], __WEBPACK_IMPORTED_MODULE_1__angular_forms__["a" /* FormBuilder */], __WEBPACK_IMPORTED_MODULE_3__angular_router__["c" /* Router */], __WEBPACK_IMPORTED_MODULE_4_ngx_bootstrap__["a" /* BsModalService */]])
    ], LoginComponent);
    return LoginComponent;
}());

var UserDto = /** @class */ (function () {
    function UserDto() {
    }
    return UserDto;
}());


/***/ }),

/***/ "../../../../../src/app/login/login.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LoginModule", function() { return LoginModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__login_component__ = __webpack_require__("../../../../../src/app/login/login.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__login_routing__ = __webpack_require__("../../../../../src/app/login/login.routing.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_shared_module__ = __webpack_require__("../../../../../src/app/shared/shared.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_ngx_bootstrap_modal__ = __webpack_require__("../../../../ngx-bootstrap/modal/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared_modals_modal_component__ = __webpack_require__("../../../../../src/app/shared/modals/modal.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};








var LoginModule = /** @class */ (function () {
    function LoginModule() {
    }
    LoginModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_4__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_2__login_routing__["a" /* LoginRoutingModule */],
                __WEBPACK_IMPORTED_MODULE_3__angular_forms__["e" /* ReactiveFormsModule */],
                __WEBPACK_IMPORTED_MODULE_6_ngx_bootstrap_modal__["c" /* ModalModule */].forRoot(),
                __WEBPACK_IMPORTED_MODULE_5__shared_shared_module__["a" /* SharedModule */]
            ],
            declarations: [__WEBPACK_IMPORTED_MODULE_1__login_component__["a" /* LoginComponent */]],
            entryComponents: [__WEBPACK_IMPORTED_MODULE_7__shared_modals_modal_component__["a" /* ModalComponent */]]
        })
    ], LoginModule);
    return LoginModule;
}());



/***/ }),

/***/ "../../../../../src/app/login/login.routing.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LoginRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__login_component__ = __webpack_require__("../../../../../src/app/login/login.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var routes = [
    {
        path: '',
        component: __WEBPACK_IMPORTED_MODULE_2__login_component__["a" /* LoginComponent */],
        data: {
            title: 'Login'
        }
    }
];
var LoginRoutingModule = /** @class */ (function () {
    function LoginRoutingModule() {
    }
    LoginRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["d" /* RouterModule */].forChild(routes)],
            exports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["d" /* RouterModule */]]
        })
    ], LoginRoutingModule);
    return LoginRoutingModule;
}());



/***/ })

});
//# sourceMappingURL=login.module.chunk.js.map
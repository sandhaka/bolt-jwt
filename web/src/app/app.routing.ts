import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {AuthGuardService} from './security/auth.guard.service';

// Import Containers
import {
  FullLayoutComponent,
  SimpleLayoutComponent
} from './containers';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '',
    component: FullLayoutComponent,
    canActivate: [AuthGuardService],
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'dashboard',
        loadChildren: './views/dashboard/dashboard.module#DashboardModule'
      },
      {
        path: 'users',
        loadChildren: './views/users/users.module#UsersModule'
      },
      {
        path: 'authorizations',
        loadChildren: './views/authorizations/authorizations.module#AuthorizationsModule'
      }
    ]
  },
  {
    path: '',
    component: SimpleLayoutComponent,
    children: [
      {
        path: 'login',
        loadChildren: './login/login.module#LoginModule'
      },
      {
        path: 'account',
        loadChildren: './account/account.module#AccountModule'
      }
    ]
  }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {AuthGuardService} from './security/auth.guard.service';
import {AuthRootGuardService} from "./security/auth.root-guard.service";

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
        loadChildren: './views/dashboard/dashboard.module#DashboardModule',
        canActivate: [AuthGuardService]
      },
      {
        path: 'users',
        loadChildren: './views/users/users.module#UsersModule',
        canActivate: [AuthGuardService]
      },
      {
        path: 'roles',
        loadChildren: './views/roles/roles.module#RolesModule',
        canActivate: [AuthGuardService]
      },
      {
        path: 'groups',
        loadChildren: './views/groups/groups.module#GroupsModule',
        canActivate: [AuthGuardService]
      },
      {
        path: 'authorizations',
        loadChildren: './views/authorizations/authorizations.module#AuthorizationsModule',
        canActivate: [AuthGuardService]
      },
      {
        path: 'configuration',
        loadChildren: './views/configuration/configuration.module#ConfigurationModule',
        canActivate: [AuthRootGuardService]
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
      },
      {
        path: 'error',
        loadChildren: './error-pages/error-pages.module#ErrorPagesModule'
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}

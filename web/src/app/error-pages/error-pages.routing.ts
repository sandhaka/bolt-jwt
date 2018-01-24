import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {ForbiddenComponent} from "./forbidden.component";

const routes: Routes = [
  {
    path: '403',
    component: ForbiddenComponent,
    data: {
      title: '403'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ErrorPagesRouting {}

import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {AuthorizationsComponent} from "./authorizations.component";

const routes: Routes = [
  {
    path: '',
    component: AuthorizationsComponent,
    data: {
      title: 'Authorizations'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorizationsRoutingModule {}

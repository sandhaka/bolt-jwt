import {RouterModule, Routes} from "@angular/router";
import {ConfigurationComponent} from "./configuration.component";
import {NgModule} from "@angular/core";

const routes: Routes = [
  {
    path: '',
    component: ConfigurationComponent,
    data: {
      title: 'Configuration'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigurationRoutingModule { }

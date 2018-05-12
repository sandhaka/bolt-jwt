import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs';
import {SecurityService} from './security.service';

/**
 * Guard to protect root routes
 */
@Injectable()
export class AuthRootGuardService implements CanActivate {

  private router: Router;
  private authService;

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
  : boolean | Observable<boolean> | Promise<boolean> {

    const authStatus = this.authService.tokenCheck();

    if (authStatus.Authenticated) {

      if(!authStatus.RootUser) {
        this.router.navigate(['/error/403']);
        return false;
      }

      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }

  constructor(router: Router, authService: SecurityService) {
    this.router = router;
    this.authService = authService;
  }
}

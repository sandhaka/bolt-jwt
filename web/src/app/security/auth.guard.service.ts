import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {SecurityService} from './security.service';

/**
 * General guard to protect routes
 */
@Injectable()
export class AuthGuardService implements CanActivate {

  private router: Router;
  private authService;

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : boolean | Observable<boolean> | Promise<boolean> {

    if (this.authService.tokenCheck()) {
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

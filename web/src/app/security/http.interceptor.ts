import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable, Injector} from "@angular/core";
import {SecurityService} from "./security.service";

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {

  constructor(private inj: Injector) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const auth = this.inj.get(SecurityService);

    if(auth.token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${auth.token}`
        }
      });
    }

    return next.handle(req)
  }
}

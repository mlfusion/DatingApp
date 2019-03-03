
import { NotificationService } from './../_services/notification.service';
import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { RoleService } from '../_services/role.service';
import { Role } from '../_models/Role';

@Injectable()
export class RoleListResolver implements Resolve<Role[]> {
    constructor(private roleService: RoleService,
                private router: Router,
                private notificationService: NotificationService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Role[]> {
        return this.roleService.getAllRoles(1, 5, '', '', '').pipe(
            catchError(error => {
                this.notificationService.error('Problem retrieving role list data');
                this.router.navigate(['/settings']);
                return of(null);
            })
        );
    }
}

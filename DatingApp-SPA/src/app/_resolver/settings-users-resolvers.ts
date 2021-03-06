import { NotificationService } from './../_services/notification.service';
import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class SettingsUsersResolver implements Resolve<User[]> {
    constructor(private userService: UserService,
                private router: Router,
                private notificationService: NotificationService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getAllUsers(1, 5, '', '', '').pipe(
            catchError(error => {
                this.notificationService.error('Problem retrieving data settings users');
                this.router.navigate(['/settings']);
                return of(null);
            })
        );
    }
}

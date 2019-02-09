import { AuthService } from './../_services/auth.service';
import { NotificationService } from '../_services/notification.service';
import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
    constructor(private userService: UserService,
                private router: Router,
                private notificationService: NotificationService,
                private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.notificationService.error('Problem retrieving data in member edit');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}

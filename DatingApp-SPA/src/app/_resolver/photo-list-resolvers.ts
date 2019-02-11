import { PhotoService } from './../_services/photo.service';
import { Photo } from './../_models/Photo';
import { NotificationService } from './../_services/notification.service';
import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable()
export class PhotoListResolver implements Resolve<Photo[]> {
    constructor(private photoService: PhotoService,
                private router: Router,
                private notificationService: NotificationService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Photo[]> {
        return this.photoService.getPhotos().pipe(
            catchError(error => {
                this.notificationService.error('Problem retrieving photo list data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}

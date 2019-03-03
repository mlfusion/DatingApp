import { AuthService } from './../_services/auth.service';
import { CanActivate } from '@angular/router/src/utils/preactivation';
import { Injectable } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements  CanActivate {
  path;
  route;
constructor(private authService: AuthService, private router: Router,
            private notificationService: NotificationService) {}

canActivate(): boolean {
  if (this.authService.loggedIn()) {
    return true;
  }

  this.notificationService.warning('You can\'t access this link. Ok');
  this.router.navigate(['/home']);
  return false;
}
}

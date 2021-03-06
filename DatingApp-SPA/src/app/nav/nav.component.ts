import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import UIkit from 'uikit';
import { NotificationService } from '../_services/notification.service';
import { Router } from '@angular/router';
import {NgForm} from '@angular/forms';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model: any = {};
photoUrl: string;

  constructor(public authService: AuthService, private notificationService: NotificationService,
              private router: Router, private spinner: Ng4LoadingSpinnerService) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe( p => this.photoUrl = p);
  }

  login() {
    this.spinner.show();
    this.authService.login(this.model).subscribe(
      next => {
        console.log('login successfully');
        this.notificationService.success('Logged in successfully');
        this.spinner.hide();
      }, error => {
        this.notificationService.error(error);
        console.error(error);
        this.spinner.hide();
      }, () => {
        this.model = {};
        this.router.navigate(['/members']);
      }
    );
  }

  loggedIn() {
   return this.authService.loggedIn();
 }

  logout() {
    localStorage.clear();
    this.notificationService.success('Logout successfully');
    console.log('logout');
    this.router.navigate(['/home']);
  }

}

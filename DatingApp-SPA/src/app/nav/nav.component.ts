import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import UIkit from 'uikit';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model: any = {};

  constructor(private authSerice: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authSerice.login(this.model).subscribe(
      next => {
        console.log('login succefully');
      }, error => {

        UIkit.notification({message: error,
      status: 'danger',
      pos: 'bottom-right'});

      console.error(error);
      }
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !! token;
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logout');
  }

}

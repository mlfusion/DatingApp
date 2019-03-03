import { AuthService } from './../../_services/auth.service';
import { UserService } from './../../_services/user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NotificationService } from 'src/app/_services/notification.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { BsModalRef, BsModalService, ModalDirective } from 'ngx-bootstrap';
import { SettingsUserViewComponent } from '../settings-user-view/settings-user-view.component';

@Component({
  selector: 'app-settings-users',
  templateUrl: './settings-users.component.html',
  styleUrls: ['./settings-users.component.css']
})
export class SettingsUsersComponent implements OnInit {
users: User[];
user: User;
searchUser: any;
canUpdate: boolean;
// Set as Child Component to call method
@ViewChild(SettingsUserViewComponent) settingsUserViewComponent: SettingsUserViewComponent;

  constructor(private notificationService: NotificationService,
              private route: ActivatedRoute, private authService: AuthService,
              private modalService: BsModalService, private userService: UserService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'];
    });
    // console.log(this.users);
  }

  getUser(user: User) {
    this.userService.getUser(user.id).subscribe(data => {
      console.log(data);
      this.user = data;
      // console.log('token id ' + this.authService.decodedToken.nameid + ', id: ' + this.user.id);
      // this.settingsUserViewComponent.canUpdate = this.authService.decodedToken.nameid == this.user.id ? false : true;
      // console.log('ngOnChanges ' + this.settingsUserViewComponent.canUpdate);

      this.settingsUserViewComponent.showChildModal();
    }, error => {
      console.error(error);
      this.notificationService.error(error);
    });
  }

  statusFromChild(event: any) {
    console.log(event);
  }

}

import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { RoleService } from 'src/app/_services/role.service';
import { AuthService } from './../../_services/auth.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { UserService } from './../../_services/user.service';
import { Component, OnInit, Input, ViewChild, Output, EventEmitter, OnChanges } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ModalDirective } from 'ngx-bootstrap';
import { Role } from 'src/app/_models/Role';

@Component({
  selector: 'app-settings-user-view',
  templateUrl: './settings-user-view.component.html',
  styleUrls: ['./settings-user-view.component.css']
})
export class SettingsUserViewComponent implements OnInit, OnChanges {
roles: Role[];
@Input() user: User;
@Output() statusFromChild = new EventEmitter();
// Set as child Modal
@ViewChild('childModal') childModal: ModalDirective;
canUpdate = true;

  constructor(private userService: UserService, private notificationService: NotificationService, private authService: AuthService,
              private roleService: RoleService, private loader: Ng4LoadingSpinnerService) {
   // console.log('constructor');
   }

  ngOnInit() {
    if (this.user != null) {
      console.log(this.user);
    }
   // console.log('ngOnInit');
  }

  // gets executed when calling child component
  ngOnChanges() {
    if (this.user != null) {
     // console.log('token id ' + this.authService.decodedToken.nameid + ', id: ' + this.user.id);
      // tslint:disable-next-line:triple-equals
      // update by userid
      // this.canUpdate = this.authService.decodedToken.nameid == this.user.id ? false : true;
      // update by role
      this.canUpdate = this.authService.decodedToken.role === 'Admin' ? false : true;
     // console.log('ngOnChanges ' + this.canUpdate);
      this.getRoles();
    }
  }

  updateUser(user: User) {

    console.log(user);

    this.loader.show();

    this.userService.updateUser(user.id, user).subscribe(() => {
      this.notificationService.success(user.knownAs + ' has been updated.');
      this.childModal.hide();
      this.loader.hide();
    }, error => {
      console.error(error);
      this.notificationService.error(error);
      this.loader.hide();
    });
  }

  getRoles() {
    this.roleService.getRoles().subscribe(r => {
      this.roles = r.data;
    }, error => {
      console.error(error);
      this.notificationService.error(error.errorMessage);
    });
  }

  showChildModal(): void {
    this.childModal.show();
  }

  hideChildModal(username: any): void {
    this.statusFromChild.emit('You pick this value ' + username);
    this.childModal.hide();
  }

}

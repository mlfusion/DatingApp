import { AuthService } from 'src/app/_services/auth.service';
import { Component, OnInit, Input, ViewChild, Output, EventEmitter, OnChanges } from '@angular/core';
import { Role } from 'src/app/_models/Role';
import { ModalDirective, BsModalService } from 'ngx-bootstrap';
import { NotificationService } from 'src/app/_services/notification.service';
import { RoleService } from 'src/app/_services/role.service';

@Component({
  selector: 'app-role-edit',
  templateUrl: './role-edit.component.html',
  styleUrls: ['./role-edit.component.css']
})
export class RoleEditComponent implements OnInit, OnChanges {
canUpdate: boolean;
dumb: boolean;
title = 'Add';
@Input() role: Role;
@Output() status = new EventEmitter();
@ViewChild('childModal') childModal: ModalDirective;
radioModel: string;

  constructor(private notificationService: NotificationService, private roleService: RoleService, private authService: AuthService) { }

  ngOnInit() {

  }

  ngOnChanges() {
    this.canUpdate = this.authService.decodedToken.role === 'Admin' ? false : true;
    console.log(this.canUpdate);
  }

  addRole(role: Role) {
    this.roleService.addRole(role).subscribe(r => {
     // console.log(r);
      this.notificationService.success('New role has been added.');
      this.hideChildModal(r.data);
    }, error => {
      console.error(error);
      this.notificationService.error(error);
    });
  }

  updateRole(role: Role) {
    this.roleService.updateRole(role).subscribe(r => {
      this.notificationService.success('Role has been updated.');
      this.hideChildModal(role);
    }, error => {
      console.error(error);
      this.notificationService.warning(error);
    });
  }

  showChildModal(): void {
    this.childModal.show();
  }

  hideChildModal(role: any): void {
   // this.statusFromChild.emit('You pick this value ' + username);
   this.status.emit(role);
   this.childModal.hide();
  }


}

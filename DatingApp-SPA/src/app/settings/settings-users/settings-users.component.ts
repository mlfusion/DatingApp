import { AuthService } from './../../_services/auth.service';
import { UserService } from './../../_services/user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NotificationService } from 'src/app/_services/notification.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { BsModalRef, BsModalService, ModalDirective } from 'ngx-bootstrap';
import { SettingsUserViewComponent } from '../settings-user-view/settings-user-view.component';
import { Pagination } from 'src/app/_models/pagination';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-settings-users',
  templateUrl: './settings-users.component.html',
  styleUrls: ['./settings-users.component.css']
})
export class SettingsUsersComponent implements OnInit {
users: User[];
user: User;
searchUser: any;
sortOrder?: any;
sortColumn?: any;
sortAsc: boolean;
pagination: Pagination;
canUpdate: boolean;
searchForm: FormGroup;
// Set as Child Component to call method
@ViewChild(SettingsUserViewComponent) settingsUserViewComponent: SettingsUserViewComponent;

  constructor(private notificationService: NotificationService,
              private route: ActivatedRoute, private authService: AuthService,
              private modalService: BsModalService, private userService: UserService,
              private formBuilder: FormBuilder ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result.data;
      this.pagination = data.users.pagination;
    });
     // console.log(this.users);

    this.createSeachForm();

    this.onChanges();
  }

  createSeachForm() {
    this.searchForm = this.formBuilder.group({
      searchUser: ['']
    });
  }

  onChanges() {
    this.searchForm.valueChanges.subscribe(r => {
      console.log(r.searchUser);
      this.sortAsc = null;
      this.sortOrder = null;
      this.sortColumn = null;
      this.searchUser = r.searchUser;
      this.getUsers();
    });

  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getUsers();
  }

  getUsers() {
    this.searchUser = this.searchUser === undefined ? '' : this.searchUser;
    // console.log('searchRole: ' + searchRole);
    this.userService.getAllUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.searchUser,
      this.sortOrder, this.sortColumn).subscribe(
      (res => {
        this.users = res.result.data;
        this.pagination = res.pagination;
      })
    );
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

  sortBy(col: any) {
    this.sortColumn = col;
    this.sortAsc = this.sortAsc ? false : true;
    this.sortOrder = this.sortAsc ? 'ASC' : 'DESC';
    this.getUsers();
    // console.log(this.sortColumn + ' is sorted by ' + this.sortOrder);
  }

}

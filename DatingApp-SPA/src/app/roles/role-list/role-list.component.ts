import { FormBuilder, FormGroup } from '@angular/forms';
import { Pagination } from './../../_models/pagination';
import { RoleEditComponent } from './../role-edit/role-edit.component';
import { RoleService } from './../../_services/role.service';
import { Component, OnInit, ViewChild, OnChanges } from '@angular/core';
import { Role } from 'src/app/_models/Role';
import { NotificationService } from 'src/app/_services/notification.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})
export class RoleListComponent implements OnInit {
roles: Role[];
role: Role;
searchRole?: any;
sortOrder?: any;
sortColumn?: any;
sortAsc: boolean;
pagination: Pagination;
searchForm: FormGroup;
@ViewChild(RoleEditComponent) roleEditComponent: RoleEditComponent;

  constructor(private notificationService: NotificationService,
              private route: ActivatedRoute, private authService: AuthService,
              private modalService: BsModalService, private roleService: RoleService,
              private formBuilder: FormBuilder ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.roles = data.roles.result.data;
      this.pagination = data.roles.pagination;
    });

    this.createSeachForm();

    this.onChanges();
  }

  createSeachForm() {
    this.searchForm = this.formBuilder.group({
      searchRole: ['']
    });
  }

  onChanges() {
    this.searchForm.valueChanges.subscribe(r => {
      // console.log(r.searchRole);
      this.sortAsc = null;
      this.sortOrder = null;
      this.sortColumn = null;
      this.searchRole = r.searchRole;
      this.getRoles();
    });

  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getRoles();
  }

  getRoles() {
    this.searchRole = this.searchRole === undefined ? '' : this.searchRole;
    // console.log('searchRole: ' + searchRole);
    this.roleService.getAllRoles(this.pagination.currentPage, this.pagination.itemsPerPage, this.searchRole,
      this.sortOrder, this.sortColumn).subscribe(
      (res => {
        this.roles = res.result.data;
        this.pagination = res.pagination;
      })
    );
  }

  addRole() {
    this.roleEditComponent.title = 'Add';
    this.role = {};
    this.roleEditComponent.dumb = true;
    this.roleEditComponent.showChildModal();
  }

  getRole(role: Role) {
    console.log(role);
    this.roleEditComponent.title = 'Update';
    this.role = role;
    this.roleEditComponent.dumb = false;
    this.roleEditComponent.showChildModal();
  }

  status(role: Role) {
    if (role != null) {
      this.roles.push(role);
    }
  }

  sortBy(col: any) {
    this.sortColumn = col;
    this.sortAsc = this.sortAsc ? false : true;
    this.sortOrder = this.sortAsc ? 'ASC' : 'DESC';
    this.getRoles();
    // console.log(this.sortColumn + ' is sorted by ' + this.sortOrder);
  }

}

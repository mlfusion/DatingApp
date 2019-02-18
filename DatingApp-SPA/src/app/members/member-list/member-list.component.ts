import { NotificationService } from '../../_services/notification.service';
import { User } from '../../_models/user';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
users: User[];

  constructor(private userService: UserService,
              private notificationService: NotificationService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    // this.getUsers();
        // Usering resolver to get user
        this.route.data.subscribe(data => {
          this.users = data["users"];
        });
  }

  // getUsers() {
  //   this.userService.getUsers().subscribe((users: User[]) => {
  //     this.users = users;
  //   }, error => {
  //     this.notificationService.error(error);
  // });
  // }

  // gettUser(id: number) {
  //   this.userService.getUser(id).subscribe((user: User) => {

  //   })
  // }

}

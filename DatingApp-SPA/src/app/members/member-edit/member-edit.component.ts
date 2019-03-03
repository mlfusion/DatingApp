import { AuthService } from './../../_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { NotificationService } from 'src/app/_services/notification.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
user: User;
warning: string;
photoUrl: string;
@ViewChild('editForm') editForm: NgForm;
// Set notification when exiting out of the browser
@HostListener('window:beforeunload', ['$event'])
unloadNotification($event: any) {
  if (this.editForm.dirty) {
    $event.returnValue = true;
  }
}

  constructor(private route: ActivatedRoute,
              private notificationService: NotificationService,
              private userService: UserService,
              private authService: AuthService) { }

  ngOnInit() {

      // Usering resolver to get user
      this.route.data.subscribe(data => {
        this.user = data['user'];
        console.log(this.user);
      });

      this.warning = ' You have made changes.  Any unsaved changes will be lost!';
      this.authService.currentPhotoUrl.subscribe( p => this.photoUrl = p);

  }

  updateUser() {
     this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {

      this.notificationService.success('Profile has been updated.');

      // Reset form without resetting form values
      this.editForm.reset(this.user);

    }, error => {
      this.notificationService.error(error);
    });

  }

  updateMainPhoto(status) {
    console.log('isMainPhoto: ' + status);
         // Usering resolver to get user
    // this.route.data.subscribe(data => {
    //       this.user = data['user'];
    //     });
  }

  updatedMainPhoto(photo) {
   // this.user.photoUrl = photo;
  }

}

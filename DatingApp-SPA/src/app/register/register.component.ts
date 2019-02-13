import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import UIkit from 'uikit';
import { NotificationService } from '../_services/notification.service';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
model: any = {};
@Output() cancelRegister = new EventEmitter();
registerForm: FormGroup;

  constructor(private authService: AuthService,
              private notificationService: NotificationService,
              private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      username: new FormControl(),
      password: new FormControl(),
      confirmPassword: new FormControl()
    });
  }

  register() {
    this.authService.register(this.model).subscribe(res => {
      console.log(res);
      this.cancelRegister.emit(false);

      this.notificationService.success('Your registeration was successful');
    }, error => {

      this.notificationService.error(error);

      console.error(error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancel');
  }

}

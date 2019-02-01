import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import UIkit from 'uikit';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
model: any = {};
@Output() cancelRegister = new EventEmitter();

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(res => {
      console.log(res);
      this.cancelRegister.emit(false);

      UIkit.notification({message: 'Your registeration was successful',
      status: 'success',
      pos: 'bottom-right'});

    }, error => {

      UIkit.notification({message: error,
      status: 'danger',
      pos: 'bottom-right'});

      console.error(error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancel');
  }

}

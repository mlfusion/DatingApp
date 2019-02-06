import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import UIkit from 'uikit';
import Icons from 'uikit/dist/js/uikit-icons';
import { NotificationService } from '../_services/notification.service';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
values: any;
ret: any;
val: any;

  constructor(private http: HttpClient, private notification: NotificationService) { }

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(
      x => {
          this.values = x;
      }, error => console.log(error)
    );
  }

  getDefault() {
    UIkit.modal.alert('default was click');
  }

  getPrimary() {
    UIkit.notification({message: 'Hello Primary',
                        status: 'primary',
                        pos: 'top-right'});
  }

  getId(id: any) {
   // UIkit.modal.alert('ID was click ' + id);
   this.val = id;
   this.model();
  }

  confirm() {
    UIkit.modal.confirm(
      'Are you sure?',
       { labels: { 'cancel': 'Nooo', 'ok': 'Yes' } }
   ).then(function() {
       console.log('Confirmed.');
   }, function() {
       console.log('Rejected.');
   });
    // this.notification.confirm('Confirm was click');
    // const confirm = localStorage.getItem('confirm');
    // console.log(confirm);
  }

  alert() {
    this.notification.alert('Alert was click');
  }

  warning() {
    this.notification.warning('Warning was click');
  }

  success() {
    this.notification.success('Success was click');
  }

  model() {
    console.log('model');
    UIkit.modal('#modal-close-default').show();
  }

}

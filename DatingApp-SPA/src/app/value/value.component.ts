import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import UIkit from 'uikit';
import Icons from 'uikit/dist/js/uikit-icons';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
values: any;

  constructor(private http: HttpClient) { }

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
    UIkit.modal.alert('ID was click ' + id);
  }

}

import { ValueComponent } from './../value.component';
import { Component, OnInit, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { validateConfig } from '@angular/router/src/config';

@Component({
  selector: 'app-value-model',
  templateUrl: './valueModel.component.html',
  styleUrls: ['./valueModel.component.css']
})
export class ValueModelComponent implements OnInit {
  @Input() vall: any;
  @Output() openSuccess = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  success() {
    this.openSuccess.emit();
  }

}

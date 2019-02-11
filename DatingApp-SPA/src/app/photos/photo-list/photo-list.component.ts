import { PhotoService } from './../../_services/photo.service';
import { Component, OnInit } from '@angular/core';
import { NotificationService } from 'src/app/_services/notification.service';
import { Photo } from 'src/app/_models/Photo';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-photo-list',
  templateUrl: './photo-list.component.html',
  styleUrls: ['./photo-list.component.css']
})
export class PhotoListComponent implements OnInit {
photos: Photo[];

  constructor(private photoService: PhotoService, notificationService: NotificationService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.photos = data.photos;
  });

}

}

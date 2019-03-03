import { PhotoService } from './../../_services/photo.service';
import { AuthService } from './../../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/_models/user';
import { Photo } from 'src/app/_models/Photo';
import { FileUploader, ParsedResponseHeaders, FileItem } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/_services/notification.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo;
  @Output() isMainPhoto = new EventEmitter<boolean>();
  @Output() updatedMainPhoto = new EventEmitter<string>();
  baseUrl = environment.apiUrl;
  uploader: FileUploader; // = new FileUploader({url: url});
  hasBaseDropZoneOver = false;
  currentMain: Photo;
  uploadedPhoto: Photo;

  constructor(private authService: AuthService, private photoService: PhotoService,
              private noticationService: NotificationService) { }

  ngOnInit() {
    this.initializeUploader();
    // console.log(this.photos);
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          created: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };

        // testing purpose
        // console.log(photo);

        if (photo.isMain) {
          // update photo with BehaviorSubject
          this.authService.changeMemberPhoto(photo.url);

          // update user stored in authSevice
          this.authService.currentusr.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authService.currentusr));
        }

        this.photos.push(photo);

      }
    };

    this.uploader.onErrorItem = ((item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any => {
      this.noticationService.error(response);
      console.error(response);
      });

  }

  setMainPhoto(photo: Photo) {
    // console.log(photo);
    // set photo = true; set old photo = false
    this.noticationService.confirm('Are you sure you want to set this photo to your main profile?', () => {
      this.photoService.setMainPhoto(photo.id).subscribe(result => {
        this.currentMain = this.photos.filter(p => p.isMain === true)[0];
        this.currentMain.isMain = false;
        photo.isMain = true;

        this.noticationService.success('Photo has been set as main photo.');

        // Code is not in user. Just for template purpose
        this.isMainPhoto.emit(true);
        this.updatedMainPhoto.emit(photo.url);

      // update photo with BehaviorSubject
        this.authService.changeMemberPhoto(photo.url);

        // update user stored in authSevice
        this.authService.currentusr.photoUrl = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authService.currentusr));

        // set photo in global service
        // this.authService.photoUrl = photo.url;
        // localStorage.setItem('photoUrl', photo.url);
      }, (error) => {
        this.noticationService.error(error);
        console.error(error);
      });
    });
  }

    deletePhoto(id: number) {

      // console.log(photo);
      // return false;

      this.noticationService.confirm('Are you sure you want to delete this photo?', () => {
        this.photoService.deletePhoto(id).subscribe(() => {

          this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
          this.noticationService.success('Photo has been deleted');

        }, error => {
          this.noticationService.error(error);
          console.error(error);
        });

        // this.photoService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => {
        //   this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        //   this.noticationService.success('Photo has been deleted');
        // }, error => {
        //   this.alertify.error('Failed to delete the photo');
        // });
      });
    }

}

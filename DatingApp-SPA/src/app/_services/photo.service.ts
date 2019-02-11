import { Photo } from 'src/app/_models/Photo';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

constructor(private http: HttpClient, private authService: AuthService) { }
baseurl = environment.apiUrl + 'users/' + this.authService.decodedToken.nameid + '/photos/';

getPhotos() {
 return this.http.get<Photo[]>(this.baseurl);
}

getPhoto(id: number) {
  return this.http.get<Photo>(this.baseurl + id);
}

setMainPhoto(id: number) {
  return this.http.post(this.baseurl + id + '/setMainPhoto', {});
}

deletePhoto(id: number) {
  return this.http.post(this.baseurl + id + '/deletePhoto', {});
}

}

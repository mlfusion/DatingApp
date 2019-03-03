import { Pipe, PipeTransform } from '@angular/core';
import { Photo } from '../_models/Photo';

@Pipe({
  name: 'filter'
})
export class PhotoSearchPipe implements PipeTransform {

  transform(photos: any[], searchPhoto: any): any[] {

    if (!photos) { return []; }
    if (!searchPhoto) { return photos; }

    searchPhoto = searchPhoto.toLowerCase();

    return photos.filter( x => {
      return x.description.toLowerCase().includes(searchPhoto);
    });

  }
 //
}

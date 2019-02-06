import { Injectable } from '@angular/core';
import UIkit from 'uikit';
import alertify from 'alertifyjs';
//declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

constructor() { }

// success(ret: any) {
//   UIkit.notification({message: ret,
//     status: 'success',
//     pos: 'bottom-right'});
// }

success(ret: any) {
  alertify.success(ret);
}

// error(ret: any) {
//   UIkit.notification({message: ret,
//     status: 'danger',
//     pos: 'bottom-right'});
// }

error(ret: any){
  alertify.error(ret);
}


// warning(ret: any) {
//   UIkit.notification({message: ret,
//     status: 'warning',
//     pos: 'bottom-right'});
// }

warning(ret: any){
  alertify.warning(ret);
}

// info(ret: any) {
//   UIkit.notification({message: ret,
//     status: 'primary',
//     pos: 'bottom-right'});
// }

info(ret: any){
  alertify.message(ret);
}


// confirm(ret: any) {
//   localStorage.removeItem('confirm');
//   UIkit.modal.confirm(ret).then(function() {
//     localStorage.setItem('confirm', 'true');
//       console.log('Confirmed.');
//   }, function() {
//     localStorage.setItem('confirm', 'false');
//       console.log('Rejected.');
//   });
// }

alert(ret: any) {
  UIkit.modal.alert(ret);
}


}

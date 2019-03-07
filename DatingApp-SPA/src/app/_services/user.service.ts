import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { ApiResult } from '../_models/api-result';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: 'Bearer ' + localStorage.getItem('token')
//   })
// };

@Injectable({
  providedIn: 'root'
})
export class UserService {
baseUrl = environment.apiUrl + 'users/';

constructor(private http: HttpClient) {}

getUsers(): Observable<User[]> {
  return this.http.get<User[]>(this.baseUrl);
  }

  getAllUsers(pageNumber?: any, itemsPerPage?: any, search?: any, sortOrder?: any, sortColumn?: any) {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    // console.log(search);

    let params = new HttpParams();

    if (pageNumber != null && itemsPerPage != null) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', itemsPerPage);
      params = params.append('search', search);
      params = params.append('sortOrder', sortOrder);
      params = params.append('sortColumn', sortColumn);
    }

    return this.http.get<ApiResult<User[]>>(this.baseUrl + 'all', { observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          console.log( paginatedResult.result);

          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
   }

getUser(id: number): Observable<User> {
  return this.http.get<User>(this.baseUrl + id);
  }

updateUser(id: number, user: User) {
  // console.log('User Service Update Called');
  return this.http.put(this.baseUrl + id, user);
}
}

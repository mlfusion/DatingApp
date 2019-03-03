import { map } from 'rxjs/operators';
import { Role } from './../_models/Role';
import { Observable } from 'rxjs';

import { AuthService } from './auth.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { ApiResult } from '../_models/api-result';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

constructor(private http: HttpClient, private authService: AuthService) { }
baseurl = environment.apiUrl + 'roles/';

getRoles() {
 return this.http.get<ApiResult<Role[]>>(this.baseurl);
}

getAllRoles(pageNumber?: any, itemsPerPage?: any, search?: any, sortOrder?: any, sortColumn?: any) {
  const paginatedResult: PaginatedResult<Role[]> = new PaginatedResult<Role[]>();

  // console.log(search);

  let params = new HttpParams();

  if (pageNumber != null && itemsPerPage != null) {
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', itemsPerPage);
    params = params.append('search', search);
    params = params.append('sortOrder', sortOrder);
    params = params.append('sortColumn', sortColumn);
  }

  return this.http.get<ApiResult<Role[]>>(this.baseurl + 'all', { observe: 'response', params})
    .pipe(
      map(response => {
        paginatedResult.result = response.body;

        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
 }

getRole(id: number) {
  return this.http.get<ApiResult<Role>>(this.baseurl + id);
}

addRole(role: Role) {
  return this.http.post<ApiResult<Role>>(this.baseurl, role);
}

updateRole(role: Role) {
  return this.http.put<ApiResult<Role>>(this.baseurl, role);
}

}

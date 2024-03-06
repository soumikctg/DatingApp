import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {IUser} from "../_models/user";

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsersWithRoles(){
    return this.http.get<IUser[]>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(username: string, roles: any){
    return this.http.post<string[]>(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }

}

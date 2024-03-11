import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { IUser } from '../_models/user';
import {environment} from "../../environments/environment";
import {PresenceService} from "./presence.service";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<IUser | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient, private presenceService: PresenceService) { }

  login(model: any){
    return this.http.post<IUser>(this.baseUrl+ 'accounts/login', model).pipe(
      map((response: IUser) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post<IUser>(this.baseUrl+'accounts/register', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: IUser){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);

    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
    this.presenceService.createHubConnection(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
  }

  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]))
  }
}

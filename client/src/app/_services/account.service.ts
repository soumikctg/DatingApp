import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { IUser } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = 'http://localhost:5000/api/';
  private currentUserSource = new BehaviorSubject<IUser | null>(null);
  currentUsers = this.currentUserSource.asObservable();
  constructor(private http: HttpClient ) { }

  login(model: any){
    return this.http.post<IUser>(this.baseUrl+ 'accounts/login', model).pipe(
      map((response: IUser) => {
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  setCurrentUser(user: IUser){
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}

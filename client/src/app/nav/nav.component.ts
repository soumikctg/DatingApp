import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Observable, of } from 'rxjs';
import { IUser } from '../_models/user';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {MembersService} from "../_services/members.service";


@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, CommonModule, BsDropdownModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent implements OnInit{
  model: any = {};
  currentUsers: Observable<IUser | null> = of(null);


  constructor(public accountService: AccountService, private router: Router, private memberService: MembersService){ }
  ngOnInit(): void {
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: _ => {
        this.router.navigateByUrl('/members')
      }
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}

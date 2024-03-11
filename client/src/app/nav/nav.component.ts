import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import {HasRoleDirective} from "../_directives/has-role.directive";



@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, CommonModule, BsDropdownModule, RouterLink, RouterLinkActive, HasRoleDirective],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent implements OnInit{
  model: any = {};

  constructor(public accountService: AccountService, private router: Router){ }
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

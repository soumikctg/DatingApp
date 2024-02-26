import {Component, OnInit} from '@angular/core';
import {Member} from "../../_models/member";
import {MembersService} from "../../_services/members.service";
import {CommonModule} from "@angular/common";
import {MemberCardComponent} from "../member-card/member-card.component";
import {Pagination} from "../../_models/pagination";
import {PaginationModule} from "ngx-bootstrap/pagination";
import {FormsModule} from "@angular/forms";
import {UserParams} from "../../_models/userParams";
import {IUser} from "../../_models/user";
import {ButtonsModule} from "ngx-bootstrap/buttons";

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [
    CommonModule,
    MemberCardComponent,
    PaginationModule,
    FormsModule,
    ButtonsModule
  ],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{

  // members$: Observable<Member[]> | undefined;
  members: Member [] = [];
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;

  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}]
  constructor(private memberService : MembersService) {
    this.userParams = this.memberService.getUserParams();
  }


  ngOnInit() {
    // this.members$ = this.memberService.getMembers();
    this.loadMembers();
  }

  loadMembers(){
    if(this.userParams) {
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe(response => {
        if(response.result && response.pagination){
          this.members = response.result;
          this.pagination = response.pagination;
        }
      })
    }

  }
  resetFilters(){
      this.userParams = this.memberService.resetUserParams();
      this.loadMembers();

  }
  pageChanged(event : any){
    if(this.userParams && this.userParams?.pageNumber !== event.page){
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }

  }
}

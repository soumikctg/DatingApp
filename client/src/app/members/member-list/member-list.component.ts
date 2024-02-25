import {Component, OnInit} from '@angular/core';
import {Member} from "../../_models/member";
import {MembersService} from "../../_services/members.service";
import {CommonModule} from "@angular/common";
import {MemberCardComponent} from "../member-card/member-card.component";
import {Observable} from "rxjs";
import {Pagination} from "../../_models/pagination";
import {PaginationModule} from "ngx-bootstrap/pagination";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [
    CommonModule,
    MemberCardComponent,
    PaginationModule,
    FormsModule
  ],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{

  // members$: Observable<Member[]> | undefined;
  members: Member [] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5;
  constructor(private memberService : MembersService) {
  }


  ngOnInit() {
    // this.members$ = this.memberService.getMembers();
    this.loadMembers();
  }

  loadMembers(){
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe(response => {
      if(response.result && response.pagination){
        this.members = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  pageChanged(event : any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadMembers();
    }

  }


}

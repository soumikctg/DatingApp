import {Component, OnInit} from '@angular/core';
import {Member} from "../_models/member";
import {MembersService} from "../_services/members.service";
import {ButtonsModule} from "ngx-bootstrap/buttons";
import {FormsModule} from "@angular/forms";
import {LoadingAttr} from "ng-gallery";
import {NgForOf, NgIf} from "@angular/common";
import {MemberCardComponent} from "../members/member-card/member-card.component";
import {Pagination} from "../_models/pagination";
import {PaginationModule} from "ngx-bootstrap/pagination";

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [
    ButtonsModule,
    FormsModule,
    NgForOf,
    MemberCardComponent,
    NgIf,
    PaginationModule
  ],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit{

  members: Member[] | undefined;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;
  constructor(private memberService: MembersService) {
  }
  ngOnInit() {
    this.loadLikes();
  }

  loadLikes(){
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe( response => {
      this.members = response.result;
      this.pagination = response.pagination
    })
  }

  pageChanged(event : any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadLikes();
    }

  }

}

import {Component, OnInit} from '@angular/core';
import {Member} from "../_models/member";
import {MembersService} from "../_services/members.service";
import {ButtonsModule} from "ngx-bootstrap/buttons";
import {FormsModule} from "@angular/forms";
import {LoadingAttr} from "ng-gallery";
import {NgForOf} from "@angular/common";
import {MemberCardComponent} from "../members/member-card/member-card.component";

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [
    ButtonsModule,
    FormsModule,
    NgForOf,
    MemberCardComponent
  ],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit{

  members: Member[] | undefined;
  predicate = 'liked'
  constructor(private memberService: MembersService) {
  }
  ngOnInit() {
    this.loadLikes();
  }

  loadLikes(){
    this.memberService.getLikes(this.predicate).subscribe( response => {
      this.members = response
    })
  }

}

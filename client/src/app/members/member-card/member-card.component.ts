import {Component, Input, OnInit} from '@angular/core';
import {Member} from "../../_models/member";
import {CommonModule} from "@angular/common";
import {RouterLink} from "@angular/router";
import {MembersService} from "../../_services/members.service";
import {ToastrService} from "ngx-toastr";
import {add} from "ngx-bootstrap/chronos";

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent implements OnInit{

  @Input() member: Member | undefined;
  constructor(private memberService: MembersService, private toastr: ToastrService) {
  }
  ngOnInit() {
  }

  addLike(member: Member){
    this.memberService.addLike(member.userName).subscribe( () => this.toastr.success('You have liked' + member.knownAs))
  }

  protected readonly add = add;
}

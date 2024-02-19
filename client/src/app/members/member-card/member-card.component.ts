import {Component, Input, OnInit} from '@angular/core';
import {Member} from "../../_models/member";
import {CommonModule} from "@angular/common";
import {RouterLink} from "@angular/router";

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
  constructor() {
  }
  ngOnInit() {
  }
}

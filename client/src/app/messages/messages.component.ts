import {Component, OnInit} from '@angular/core';
import {Message} from "../_models/message";
import {Pagination} from "../_models/pagination";
import {MessageService} from "../_services/message.service";
import {NgForOf, NgIf, TitleCasePipe} from "@angular/common";
import {ButtonsModule} from "ngx-bootstrap/buttons";
import {FormsModule} from "@angular/forms";
import {TimeagoModule} from "ngx-timeago";
import {PaginationModule} from "ngx-bootstrap/pagination";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [
    NgForOf,
    ButtonsModule,
    FormsModule,
    NgIf,
    TitleCasePipe,
    TimeagoModule,
    PaginationModule,
    RouterLink
  ],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit{
  messages?: Message[];
  pagination?: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;

  constructor(private messageService: MessageService) {
  }
  ngOnInit() {
    this.loadMessages();
  }

  loadMessages(){
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response =>{
      this.messages = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }
}

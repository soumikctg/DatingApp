import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Message} from "../../_models/message";
import {MessageService} from "../../_services/message.service";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {TimeagoModule} from "ngx-timeago";
import {FormsModule, NgForm} from "@angular/forms";

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    TimeagoModule,
    FormsModule,
    AsyncPipe
  ],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements OnInit{
  @ViewChild('messageForm') messageForm? : NgForm
  @Input() username?: string;
  messageContent = '';

  constructor(public messageService : MessageService) {
  }
  ngOnInit(): void {
  }

  sendMessage(){
    if(!this.username) return;
    this.messageService.sendMessage(this.username, this.messageContent).then( () => {
     this.messageForm?.reset();
    })
  }

}
